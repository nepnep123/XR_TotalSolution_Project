using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Unity.WebRTC;



namespace WebRTCWrapper
{
	public class Communicator : MonoBehaviour
	{
		public enum DenyType
		{
			DenyByCallee,
			DuringCall,
			Timeout,
		}

		public const string SIGNALING_SERVER = "http://211.54.146.2:22022/";
		public const string STUN_SERVER = "stun:211.54.146.2";
		public const string TURN_SERVER = "turn:211.54.146.2";
		public const RTCIceCredentialType TURN_CREDENTIAL_TYPE = RTCIceCredentialType.Password;
		public const string TURN_USERNAME = "xruser";
		public const string TURN_CREDENTIAL = "xruser2023";
		
		public static RTCConfiguration RTC_CONFIG;
		public string MY_ID;

		Dictionary<string, Peer> peer_list;


		Texture no_camera_texture;
		Texture source_tex;
		RenderTexture input_video;
		AudioSource input_audio;

		VideoStreamTrack input_video_stream_track;
		AudioStreamTrack input_audio_stream_track;


		public bool Initialized{ get; private set; } = false;

		public delegate void DelegateOnCallChecked(string caller_id);
		public event DelegateOnCallChecked OnCallChecked;
		public delegate void DelegateOnConnected(string remote_id);
		public event DelegateOnConnected OnConnected;
		public delegate void DelegateOnCallDenied(string remote_id, DenyType deny_type);
		public event DelegateOnCallDenied OnCallDenied;
		public delegate void DelegateOnChatReceived(string remote_id, string chat);
		public event DelegateOnChatReceived OnChatReceived;
		public delegate void DelegateOnDisconnected(string remote_id);
		public event DelegateOnDisconnected OnDisconnected;


		private void Update()
		{
			if(source_tex != null)
			{
				Graphics.Blit(source_tex, input_video);
			}
		}

		public void Initialize(string my_id)
		{
			//RTC, Signaling
			this.MY_ID = my_id;
			RTCIceServer stun_server = new RTCIceServer();
			stun_server.urls = new[] { STUN_SERVER };
			RTCIceServer turn_server = new RTCIceServer();
			turn_server.urls = new[] { TURN_SERVER };
			turn_server.credentialType = RTCIceCredentialType.Password;
			turn_server.username = "xruser";
			turn_server.credential = "xruser2023";

			RTCIceTransportPolicy policy = RTCIceTransportPolicy.All;

			RTCConfiguration config = default;
			config.iceServers = new[] {stun_server, turn_server};
			config.iceTransportPolicy = policy;
			config.iceCandidatePoolSize = 10;
			RTC_CONFIG = config;

			peer_list = new Dictionary<string, Peer>();


			//media stream
			no_camera_texture = Resources.Load<Texture2D>("nocamera");
			input_video = new RenderTexture(1280, 720, 0, WebRTC.GetSupportedRenderTextureFormat(SystemInfo.graphicsDeviceType));
			Graphics.Blit(no_camera_texture, input_video);
			Resources.UnloadAsset(no_camera_texture);
			input_video_stream_track = new VideoStreamTrack(input_video);

			if(gameObject.GetComponent<AudioSource>())
				input_audio = gameObject.GetComponent<AudioSource>();
            else
				input_audio = gameObject.AddComponent<AudioSource>();

			input_audio.loop = true;
			input_audio_stream_track = new AudioStreamTrack(input_audio);



			StartCoroutine(WebRTC.Update());

			Initialized = true;
		}

		public void CallChecking()
		{
			StartCoroutine(CallCheckingCoroutine());
		}

		IEnumerator CallCheckingCoroutine()
		{
			UnityWebRequest web_req = UnityWebRequest.Get(SIGNALING_SERVER + "CallChecking/" + MY_ID);
			
			yield return web_req.SendWebRequest();
			
			byte[] received_data;
			long response_code = GetReceivedData(web_req, out received_data);

			if(response_code == 200)
			{
				string caller_id = (received_data == null)? null: Encoding.UTF8.GetString(received_data);
				OnCallChecked(caller_id);
			}
			
			
			web_req.Dispose();
		}

		public void Call(string callee_id)
		{
			if(peer_list.ContainsKey(callee_id))
			{
				Debug.LogWarning("�̹� Call ��û�� �߽��ϴ�. id : " + callee_id);
				return;
			}

			StartCoroutine(CallCoroutine(callee_id));
		}

		IEnumerator CallCoroutine(string callee_id)
		{
			//Create Peer
			GameObject obj = new GameObject(callee_id + "-Peer");
			obj.transform.SetParent(this.transform);
			Peer peer = obj.AddComponent<Peer>();
			peer.Initialize(RTC_CONFIG, callee_id, input_video_stream_track, input_audio_stream_track);
			peer.OnConnected += OnConnectedInternal;
			peer.OnDataReceived += OnDataReceivedInternal;
			peer.OnDisconnected += OnDisconnectedInternal;
			peer_list.Add(callee_id, peer);


			yield return peer.CreateOffer();

			yield return new WaitForSeconds(1.0f);  //ICE Candidate�� Ȯ���� ����� �ð��� �����մϴ�
			


			//Send Offer
			Peer.SignalingData signaling_data = peer.GetSignalingData();
			byte[] json_post_data = new Peer.SignalingDataPostFormat(MY_ID, ref signaling_data).SerializeAndEncodeUTF8();

			byte[] received_data;
			UnityWebRequest web_req = new UnityWebRequest(SIGNALING_SERVER + "Call/" + callee_id, "POST");
			
			web_req.uploadHandler = new UploadHandlerRaw(json_post_data);
			web_req.downloadHandler = new DownloadHandlerBuffer();
			yield return web_req.SendWebRequest();


			//Answer Process
			long response_code = GetReceivedData(web_req, out received_data);
			web_req.Dispose();
			
			if(response_code == 200)
			{
				string answer_data = Encoding.UTF8.GetString(received_data);
				//Peer.SignalingData answer_signaling_data = JsonUtility.FromJson<Peer.SignalingData>(answer_data);
				if(answer_data == "Denied")
				{
					OnCallDenied?.Invoke(callee_id, DenyType.DenyByCallee);
					Destroy(peer.gameObject);
					peer_list.Remove(callee_id);
					yield break;
				}
				else if(answer_data == "During a call")
				{
					OnCallDenied?.Invoke(callee_id, DenyType.DuringCall);
					Destroy(peer.gameObject);
					peer_list.Remove(callee_id);
					yield break;
				}
				else if(answer_data == "Timeout")
				{
					OnCallDenied?.Invoke(callee_id, DenyType.Timeout);
					Destroy(peer.gameObject);
					peer_list.Remove(callee_id);
					yield break;
				}

				yield return peer.ReceiveAnswer(answer_data);

			}
			
		}

		public void AcceptCall(string caller_id)
		{
			if (peer_list.ContainsKey(caller_id))
			{
				Debug.LogWarning("�̹� Call ��û�� �߽��ϴ�. id : " + caller_id);
				return;
			}

			StartCoroutine(AcceptCoroutine(caller_id));
			
			
		}

		IEnumerator AcceptCoroutine(string caller_id)
		{
			UnityWebRequest web_req = UnityWebRequest.Get(SIGNALING_SERVER + "SDP-Candidate/" + MY_ID);
			yield return web_req.SendWebRequest();
			byte[] received_signaling_data;
			long response_code = GetReceivedData(web_req, out received_signaling_data);
			web_req.Dispose();

			if(response_code != 200)
			{
				Debug.Log("Failed to receive signaling data");
				yield break;
			}
			
			string offer_data = Encoding.UTF8.GetString(received_signaling_data);
			Peer.SignalingData signaling_data = JsonUtility.FromJson<Peer.SignalingData>(offer_data);


			GameObject obj = new GameObject(caller_id + "-Peer");
			obj.transform.SetParent(this.transform);
			Peer peer = obj.AddComponent<Peer>();
			peer.Initialize(RTC_CONFIG, caller_id, input_video_stream_track, input_audio_stream_track);
			peer.OnConnected += OnConnectedInternal;
			peer.OnDataReceived += OnDataReceivedInternal;
			peer.OnDisconnected += OnDisconnectedInternal;
			peer_list.Add(caller_id, peer);

			yield return peer.CreateAnswer(signaling_data);
			
			yield return new WaitForSeconds(1.0f);  //ICE Candidate�� Ȯ���� ����� �ð��� �����մϴ�
			

			//Send Answer
			Peer.SignalingData answer_signaling_data = peer.GetSignalingData();
			byte[] json_post_data = new Peer.SignalingDataPostFormat(MY_ID, ref answer_signaling_data).SerializeAndEncodeUTF8();

			//byte[] received_data;
			web_req = new UnityWebRequest(SIGNALING_SERVER + "CallAcceptance/" + MY_ID, "POST");
			
			web_req.uploadHandler = new UploadHandlerRaw(json_post_data);
			web_req.downloadHandler = new DownloadHandlerBuffer();
			yield return web_req.SendWebRequest();
			byte[] received_data;
			response_code = GetReceivedData(web_req, out received_data);
			web_req.Dispose();


		}

		//void SuccessGetSDPCandidate(string json_data)
		//{
		//	Debug.Log("SuccessGetSDPCandidate: " + json_data);

		//	SignalingData signaling_data = JsonUtility.FromJson<SignalingData>(json_data);
		
		//	int num_ice_candidate = signaling_data.ice_candidate_list.Length;
		//	RTCIceCandidate[] caller_ice_candidate_list = new RTCIceCandidate[num_ice_candidate];
		//	for(int i0 = 0; i0 < num_ice_candidate; ++i0)
		//	{
		//		RTCIceCandidate ice_candidate = signaling_data.ice_candidate_list[i0].AssembleIceCandidate();
		//		caller_ice_candidate_list[i0] = ice_candidate;
		//	}


		//	StartCoroutine(Answer(signaling_data.AssembleRefSessionDescription(), caller_ice_candidate_list));
		//}


		public void DenyCall(string caller_id)
		{
			StartCoroutine(DenyCoroutine(caller_id));
		}

		IEnumerator DenyCoroutine(string caller_id)
		{
			string send_message = "Deny";
			byte[] bytes = Encoding.UTF8.GetBytes(send_message);

			byte[] received_data;
			UnityWebRequest web_req = new UnityWebRequest(SIGNALING_SERVER + "CallDenial/" + MY_ID, "POST");
			
			web_req.uploadHandler = new UploadHandlerRaw(bytes);
			web_req.downloadHandler = new DownloadHandlerBuffer();
			// Request and wait for the desired page.
			yield return web_req.SendWebRequest();

			GetReceivedData(web_req, out received_data);

			web_req.Dispose();


		}

		//상대방이 수락됬을때 
		void OnConnectedInternal(string remote_id)
		{
			OnConnected?.Invoke(remote_id);
		}

		void OnDataReceivedInternal(string remote_id, byte[] bytes)
		{
			string chat = Encoding.UTF8.GetString(bytes);
			OnChatReceived?.Invoke(remote_id, chat);
		}
		
		void OnDisconnectedInternal(string remote_id)
		{
			Destroy(peer_list[remote_id].gameObject);
			peer_list.Remove(remote_id);
			OnDisconnected?.Invoke(remote_id);
		}



		long GetReceivedData(UnityWebRequest web_req, out byte[] data)
		{
			switch (web_req.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + web_req.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + web_req.error);
					break;
				case UnityWebRequest.Result.Success:
					if(web_req.downloadHandler.data != null)
					{
						Debug.Log("Success: " + Encoding.UTF8.GetString(web_req.downloadHandler.data));
					}
					break;
			}
			
			data = web_req.downloadHandler.data;
			return web_req.responseCode;
		}



		public Texture GetVideo(string remote_id)
		{
			return peer_list[remote_id].OutputVideo;
		}
		public AudioSource GetAudio(string remote_id)
		{
			return peer_list[remote_id].OutputAudio;
		}

		public void SetVideo(Texture video)
		{
			
			if(video == null)
			{
				Debug.Log("set no camera");
				source_tex = no_camera_texture;
			}
			else
			{	
				Debug.Log(video.name);
				source_tex = video;
			}
			
		}

		public void SetAudio(AudioClip audio)
		{
			input_audio.clip = audio;
			input_audio.Play();
		}

		public void SendChatAllRemote(string chat)
		{
			foreach(var peer in peer_list.Values)
			{
				peer.SendData(chat);
			}
		}



		

		public void Close()
		{
			foreach(var peer in peer_list.Values)
			{
				Destroy(peer.gameObject);
				peer.Close();
			}
			peer_list.Clear();
		}

		
		
	}
}