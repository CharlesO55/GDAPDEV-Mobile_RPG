using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestNotifier
{
    public void NotifyQuestManager(GameObject sender, EnumQuestAction actionOccured);
}
