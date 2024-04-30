using Agora.Rtc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    string appID = "67742fa2d0f14d529516e31eccb279d4";

    public static VoiceManager Instance;

    IRtcEngine rtcEngine;

     

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        rtcEngine = RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = appID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        context.areaCode = AREA_CODE.AREA_CODE_GLOB; 
        rtcEngine.Initialize(context);
        rtcEngine.EnableAudio();
        rtcEngine.InitEventHandler(new UserEventHandler());
        rtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        rtcEngine.JoinChannel(appID, "test", "", 0);
    }

    private void OnDestroy()
    {
        rtcEngine.LeaveChannel();
        rtcEngine.Dispose();
    }

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        
        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }
        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            Debug.Log("OnUserOffline");
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            Debug.Log("OnUserJoined");
        }
    }
}
