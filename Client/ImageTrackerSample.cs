/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using maxstAR;
using System;

public class ImageTrackerSample : ARBehaviour
{
	private Dictionary<string, ImageTrackableBehaviour> imageTrackablesMap =
		new Dictionary<string, ImageTrackableBehaviour>();

	[SerializeField] private List<TrackableInfo> trackableInfos = new List<TrackableInfo>();

	private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;

	bool isARTracking = false;

	LayoutController lc;

	public void TrackStart(bool trigger)
	{
		isARTracking = trigger;
		DisableAllTrackables();
	}

	void Awake()
	{
		Init();

		cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
		if (cameraBackgroundBehaviour == null)
		{
			Debug.LogError("Can't find CameraBackgroundBehaviour.");
			return;
		}

		lc = FindObjectOfType<LayoutController>();
	}

	private void Start()
	{
		StartCoroutine(CoInit());
	}

	IEnumerator CoInit()
	{
		yield return new WaitForSeconds(0.1f);
		//Debug.Log("---------------------------------------------START IMAGETRACKERSAMPLE---------------------------------------------");

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		imageTrackablesMap.Clear();
		ImageTrackableBehaviour[] imageTrackables = FindObjectsOfType<ImageTrackableBehaviour>();
		foreach (var trackable in imageTrackables)
		{
			imageTrackablesMap.Add(trackable.TrackableName, trackable);
			trackableInfos.Add(trackable.GetComponent<TrackableInfo>());
			//Debug.Log("Trackable add: " + trackable.TrackableName);
		}

		AddTrackerData();
		TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_IMAGE);
		StartCamera();

		// For see through smart glass setting
		if (ConfigurationScriptableObject.GetInstance().WearableType == WearableCalibration.WearableType.OpticalSeeThrough)
		{
			WearableManager.GetInstance().GetDeviceController().SetStereoMode(true);

			CameraBackgroundBehaviour cameraBackground = FindObjectOfType<CameraBackgroundBehaviour>();
			cameraBackground.gameObject.SetActive(false);

			WearableManager.GetInstance().GetCalibration().CreateWearableEye(Camera.main.transform);

			// BT-300 screen is splited in half size, but R-7 screen is doubled.
			if (WearableManager.GetInstance().GetDeviceController().IsSideBySideType() == true)
			{
				// Do something here. For example resize gui to fit ratio
			}
		}

		StartCoroutine("Update_Cam");
	}

	private void AddTrackerData()
	{
		foreach (var trackable in imageTrackablesMap)
		{
			if (trackable.Value.TrackerDataFileName.Length == 0)
			{
				continue;
			}

			if (trackable.Value.StorageType == StorageType.AbsolutePath)
			{
				TrackerManager.GetInstance().AddTrackerData(trackable.Value.TrackerDataFileName);
				TrackerManager.GetInstance().LoadTrackerData();
			}
			else if (trackable.Value.StorageType == StorageType.StreamingAssets)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					StartCoroutine(MaxstARUtil.ExtractAssets(trackable.Value.TrackerDataFileName, (filePah) =>
					{
						TrackerManager.GetInstance().AddTrackerData(filePah, false);
						TrackerManager.GetInstance().LoadTrackerData();
					}));
				}
				else
				{
					TrackerManager.GetInstance().AddTrackerData(Application.streamingAssetsPath + "/" + trackable.Value.TrackerDataFileName);
					TrackerManager.GetInstance().LoadTrackerData();
				}
			}
		}
	}

	private void DisableAllTrackables()
	{
		foreach (var trackable in imageTrackablesMap)
		{
			trackable.Value.OnTrackFail();
		}
	}

	public IEnumerator Update_Cam()
	{
		//Debug.Log("Update_Cam START");

		while (true)
		{
			//DisableAllTrackables();

			TrackingState state = TrackerManager.GetInstance().UpdateTrackingState();

			cameraBackgroundBehaviour.UpdateCameraBackgroundImage(state);

			if (state == null)
			{
				yield return null;
				continue;
			}

			if (!isARTracking)
			{
				yield return null;
				continue;
			}

			TrackingResult trackingResult = state.GetTrackingResult();

			//Debug.Log("-------------------- : ");

			for (int i = 0; i < trackingResult.GetCount(); i++)
			{
				Trackable trackable = trackingResult.GetTrackable(i);

				imageTrackablesMap[trackable.GetName()].OnTrackSuccess(
					trackable.GetId(), trackable.GetName(), trackable.GetPose());

				//Debug.Log("trackableInfos : " + trackable.GetName());

				for (int j = 0; j < trackableInfos.Count; j++)
				{
					if (trackable.GetName() == trackableInfos[j].name)
					{
                        lc.ARTargetSuccess(trackableInfos[j]._type);

						TrackStart(false);
					}
				}
			}

			yield return null;
		}
	}

	public void SetNormalMode()
	{
		TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.NORMAL_TRACKING);
	}

	public void SetExtendedMode()
	{
		TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.EXTEND_TRACKING);
	}

	public void SetMultiMode()
	{
		TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.MULTI_TRACKING);
	}

	void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			TrackerManager.GetInstance().StopTracker();
			StopCamera();
		}
		else
		{
			StartCamera();
			TrackerManager.GetInstance().StartTracker(TrackerManager.TRACKER_TYPE_IMAGE);
		}
	}

	void OnDestroy()
	{
		imageTrackablesMap.Clear();
		TrackerManager.GetInstance().SetTrackingOption(TrackerManager.TrackingOption.NORMAL_TRACKING);
		TrackerManager.GetInstance().StopTracker();
		TrackerManager.GetInstance().DestroyTracker();
		StopCamera();
	}
}