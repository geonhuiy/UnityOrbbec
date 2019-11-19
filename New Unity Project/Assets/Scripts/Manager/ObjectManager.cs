using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectManager
{   
    //list of objects
    static private List<Object> objectList = new List<Object>();
    // random generator from System library
    static private System.Random rnd = new System.Random();
    static ObjectManager() {
        // declare objects
        var chair = new Object("chair");
        var egg = new Object("egg");
        var tv = new Object("tv");
        var tomato = new Object("tomato");
        var oven = new Object("oven");
        var meat = new Object("meat");

        //add object to object list
        objectList.Add(chair);
        objectList.Add(egg);
        objectList.Add(tv);
        objectList.Add(tomato);
        objectList.Add(oven);
        objectList.Add(meat);
    }

    //get list of objects in list form
    static List<Object> GetObjectList(){
        return objectList;
    }

    //get random single object from list
    static Object GetRandomObjectFromList(List<Object> list){
        return objectList[rnd.Next(0, list.Count)];
    }

    // check if Object item is in List<Object> list, return a boolean
    static bool checkObjectInList(Object item, List<Object> list){
        bool contain = false;
        if(list.Contains(item)){
            contain = true;
        } else {
            contain = false;
        }
        return contain;
    }

    //get list of random objects with possible duplicate
    static List<Object> GetRandomObjectWithDuplicate(int size){
        List<Object> newList = new List<Object>();
        for (int i = 0; i < size; i++)
        {
            newList.Add(GetRandomObjectFromList(objectList));
        }
        return newList;
    }

    //get list of random non duplicate objects
    static List<Object> GetRandomObjectWithoutDuplicate(int size){
        List<Object> newList = new List<Object>();
        List<Object> tempList = objectList;
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
