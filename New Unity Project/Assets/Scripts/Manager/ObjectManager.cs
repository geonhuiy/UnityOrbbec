using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectManager
{   
    //list of objects
    static private List<string> objectList = new List<string>();
    // random generator from System library
    static private System.Random rnd = new System.Random();
    static ObjectManager() {
        //add object to object list
        objectList.Add("chair");
        objectList.Add("egg");
        objectList.Add("tv");
        objectList.Add("tomato");
        objectList.Add("oven");
        objectList.Add("meat");
    }

    //get list of objects in list form
    static List<string> GetObjectList(){
        return objectList;
    }

    //get random single object from list
    static string GetRandomObjectFromList(List<string> list){
        return objectList[rnd.Next(0, list.Count)];
    }

    // check if Object item is in List<Object> list, return a boolean
    static bool checkObjectInList(string item, List<string> list){
        bool contain = false;
        if(list.Contains(item)){
            contain = true;
        } else {
            contain = false;
        }
        return contain;
    }

    //get list of random objects with possible duplicate
    public static List<string> GetRandomObjectWithDuplicate(int size){
        List<string> newList = new List<string>();
        for (int i = 0; i < size; i++)
        {
            newList.Add(GetRandomObjectFromList(objectList));
        }
        return newList;
    }

    //get list of random non duplicate objects
    public static List<string> GetRandomObjectWithoutDuplicate(int size){
        List<string> newList = new List<string>();
        List<string> tempList = objectList;
        // check request list size is greater than object list count, set it to object list count
        if(size > objectList.Count)  {
            size = objectList.Count;
        }

        for (int i = 0; i < size; i++)
        {
            var tempItem = GetRandomObjectFromList(tempList);
            tempList.Remove(tempItem);
            newList.Add(tempItem);
        }
        return newList;
    }
}
