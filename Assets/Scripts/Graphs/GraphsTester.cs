using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Methods
{
    All,
    Any,
    Contains,
    ContainsCompared,
    Distinct,
    DistinctCompared,
    ElementAt,
    Except,
    ExceptCompared,
    First,
    Last,
    Intersect,
    IntersectCompared,
    Count,
    SequenceEqual,
    Single,
    SkipWhile,
    Union,
    UnionCompared,
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
            case Methods.ContainsCompared:
                TestContainsCompared();
                break;
            case Methods.Distinct:
                TestDistinct();
                break;
            case Methods.DistinctCompared:
                TestDistinctCompared();
                break;
            case Methods.ElementAt:
                TestElementAt();
                break;
            case Methods.Except:
                TestExcept();
                break;
            case Methods.ExceptCompared:
                TestExceptCompared();
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
            case Methods.IntersectCompared:
                TestIntersectCompared();
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
            case Methods.UnionCompared:
                TestUnionCompared();
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
    }

    private void TestContainsCompared()
    {
    }

    private void TestDistinct()
    {
    }

    private void TestDistinctCompared()
    {
    }

    private void TestElementAt()
    {
    }

    private void TestExcept()
    {
    }

    private void TestExceptCompared()
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

    private void TestIntersectCompared()
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

    private void TestUnionCompared()
    {
    }

    private void TestWhere()
    {
    }
}