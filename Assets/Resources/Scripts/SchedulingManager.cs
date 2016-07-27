using UnityEngine;
using System.Collections.Generic;

public class SchedulingManager : Manager<SchedulingManager>
{
    private Dictionary<ScheduleType, GameObject> scheduleDic;
    private Schedule[] scheduleList;

	// Use this for initialization
	void Start ()
    {
        scheduleDic = new Dictionary<ScheduleType, GameObject>();

        GameObject[] objs = Resources.LoadAll<GameObject>("Prefabs/Schedules/");
        
        for(int i = 0; i < objs.Length; i += 1)
        {
            Schedule schedule = objs[i].GetComponent<Schedule>();
            scheduleDic.Add(schedule.Type, objs[i]);
        }

        scheduleList = new Schedule[24];
        for(int i = 0; i < 24; i += 1)
        {
            scheduleList[i] = null;
        }
	}

    //
    // 요약:
    //    스케줄을 설정하기 위한 메서드
    //
    // 매개변수:
    //    time: 1~24시까지의 시간 중 스케줄할 시간
    //
    //    type: 스케줄의 타입
    //
    public void SetSchedule(int time, ScheduleType type)
    {
        GameObject obj = null;
        if(scheduleDic.TryGetValue(type, out obj))
        {
            GameObject ins = Instantiate<GameObject>(obj);

            scheduleList[time - 1] = ins.GetComponent<Schedule>();
        }
        else
        {
            Debug.LogError("Type " + type + "object DOESN'T EXIST on prefab table");
        }
    }
}
