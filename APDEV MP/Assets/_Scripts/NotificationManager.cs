using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using Unity.VisualScripting;
using UnityEngine;

public static class NotificationManager
{
    private static string _notifacationChannelID = "Default";
    private static AndroidNotificationChannel _notificationChannel;
    private static void BuildNotificationChannel(string title = "RPG Game", string description = "Notifications")
    {
        Debug.Log("Built notif channel");

        _notificationChannel = new AndroidNotificationChannel(_notifacationChannelID, title, description, Importance.High);
        AndroidNotificationCenter.RegisterNotificationChannel(_notificationChannel);
    }

    public static void SendNotification(string title, string message)
    {
        
        if (string.IsNullOrEmpty(_notificationChannel.Name))
        {
            BuildNotificationChannel();
        }

        Debug.Log("Sending notification");


        System.DateTime time = System.DateTime.Now;

        AndroidNotification notification = new AndroidNotification(title, message, time);
        AndroidNotificationCenter.SendNotification(notification, _notifacationChannelID);
    }
}
