using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Methods
{
    All,
    Any,
    Contains,
    Distinct,
    ElementAt,
    Except,
    First,
    Last,
    Intersect,
    Count,
    SequenceEqual,
    Single,
    SkipWhile,
    Union,
    Where
}

public class GraphsTester : MonoBehaviour
{
    [SerializeField] private List<int> source1 = new List<int>();
    [SerializeField] private List<int> source2 = new List<int>();
    [SerializeField] private Methods method;
    [SerializeField] private int index;

    [ContextMenu("Test Method")]
    public void TestMethod()
    {
        switch (method)
        {
            case Methods.All:
                TestAll();
                break;
            case Methods.Any:
                TestAny();
                break;
            case Methods.Contains:
                TestContains();
                break;
            case Methods.Distinct:
                TestDistinct();
                break;
            case Methods.ElementAt:
                TestElementAt();
                break;
            case Methods.Except:
                TestExcept();
                break;
            case Methods.First:
                TestFirst();
                break;
            case Methods.Last:
                TestLast();
                break;
            case Methods.Intersect:
                TestIntersect();
                break;
            case Methods.Count:
                TestCount();
                break;
            case Methods.SequenceEqual:
                TestSequenceEqual();
                break;
            case Methods.Single:
                TestSingle();
                break;
            case Methods.SkipWhile:
                TestSkipWhile();
                break;
            case Methods.Union:
                TestUnion();
                break;
            case Methods.Where:
                TestWhere();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void TestAll()
    {
        Debug.Log($"All One: {GraphMethods.All(source1, i => i == 1)}");
    }

    private void TestAny()
    {
        Debug.Log($"Any Zero: {GraphMethods.Any(source1, i => i == 0)}");
    }

    private void TestContains()
    {
        Debug.Log($"Contains Zero: {GraphMethods.Contains(source1, 0)}");
    }

    private void TestDistinct()
    {
        List<int> distinctInts = GraphMethods.ToList(GraphMethods.Distinct(source1));
        string distincts = "Distincts: ";
        for (int i = 0; i < distinctInts.Count; i++)
        {
            distincts += $"{distinctInts[i]}, ";
        }

        Debug.Log(distincts);
    }

    private void TestElementAt()
    {
        Debug.Log($"Element at: {index}: {GraphMethods.ElementAt(source1, index)}");
    }

    private void TestExcept()
    {
        List<int> exceptsList = GraphMethods.ToList(GraphMethods.Except(source1, source2));
        string excepts = "Excepts: ";
        for (int i = 0; i < exceptsList.Count; i++)
        {
            excepts += $"{exceptsList[i]}, ";
        }

        Debug.Log(excepts);
    }

    private void TestFirst()
    {
    }

    private void TestLast()
    {
    }

    private void TestIntersect()
    {
    }

    private void TestCount()
    {
    }

    private void TestSequenceEqual()
    {
    }

    private void TestSingle()
    {
    }

    private void TestSkipWhile()
    {
    }

    private void TestUnion()
    {
    }

    private void TestWhere()
    {
    }
}