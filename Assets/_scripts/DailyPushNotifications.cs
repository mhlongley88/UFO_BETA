using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_Android
 using Unity.Notifications.Android;
#endif
#if UNITY_IPHONE
 using Unity.Notifications.iOS;
#endif


public class DailyPushNotifications : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
#if UNITY_IPHONE
 StartCoroutine(RequestAuthorization());
#endif
    }

    public void ActivatePushNotifications()
    {
        //switch (Application.platform)
        //{
        //    case RuntimePlatform.IPhonePlayer:
        //        PushNotesForIOS();
        //        break;

        //    case RuntimePlatform.Android:
        //        PushNotesForAndroid();
        //        break;
        //}

#if UNITY_IPHONE
 PushNotesForIOS();
#endif

#if UNITY_Android
 PushNotesForAndroid();
#endif
        
    }
#if UNITY_Android
    void PushNotesForAndroid()
    {

        // Register Notifications Channel - Default in our case
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // Send Notification
        var notification = new AndroidNotification();
        notification.Title = "Reward";
        notification.Text = "Congratulations! You are awarded a Reward. Click here to collect.";
        //notification.FireTime = System.DateTime.Now.AddMinutes(1);

        //System.DateTime datetime = new System.DateTime();
        //datetime.hou

        notification.FireTime = System.DateTime.Now.AddMinutes(10);
        //notification.RepeatInterval = System.TimeSpan.FromHours(1);

        //notification.SmallIcon = "my_custom_icon_id";
        //notification.LargeIcon = "my_custom_large_icon_id";


        AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }
#endif

    // IOS
#if UNITY_IPHONE
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
    }


    void PushNotesForIOS()
    {
        
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {

            TimeInterval = new System.TimeSpan(0, 2, 0),
            Repeats = false
        };

        var calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            // Year = 2020,
            // Month = 6,
            //Day = 1,
            //Hour = 1,
            Minute = 10,
            // Second = 0
            //Repeats = true
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "_notification_01",
            Title = "Reward",
            Body = "Scheduled at: " + System.DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "Congratulations! You are awarded a Reward. Click here to collect.",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = calendarTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);

        


    }
#endif
    // Update is called once per frame
    void Update()
    {
        
    }
}
