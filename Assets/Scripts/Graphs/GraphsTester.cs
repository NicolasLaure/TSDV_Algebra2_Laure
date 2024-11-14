using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<int> list = new List<int>();

    [SerializeField] private Methods method;

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
        Debug.Log($"All One: {GraphMethods.All(list, i => i == 1)}");
    }

    private void TestAny()
    {
        Debug.Log($"Any Zero: {GraphMethods.Any(list, i => i == 0)}");
    }

    private void TestContains()
    {
        Debug.Log($"Contains Zero: {GraphMethods.Contains(list, 0)}");
    }

    private void TestDistinct()
    {
        List<int> distinctInts = GraphMethods.ToList(GraphMethods.Distinct(list));
        string distincts = "Distincts: ";
        for (int i = 0; i < distinctInts.Count; i++)
        {
            distincts += $"{distinctInts[i]}, ";
        }

        Debug.Log(distincts);
    }

    private void TestElementAt()
    {
    }

    private void TestExcept()
    {
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