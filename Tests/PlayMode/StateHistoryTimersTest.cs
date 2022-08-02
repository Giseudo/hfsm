using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using HFSM;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class StateHistoryTimersTest
{
    private StateHistoryTimers _timers = new StateHistoryTimers();

    [UnityTest]
    public IEnumerator ShouldUpdateLastTimer()
    {
        Assert.AreEqual(_timers.CurrentTime, 0f);

        yield return new WaitForSeconds(.5f);

        _timers.Update();

        Assert.LessOrEqual(.5f, _timers.CurrentTime);

        _timers.ActiveIndex++;

        Assert.AreEqual(_timers.CurrentTime, 0f);

        yield return new WaitForSeconds(.5f);

        _timers.Update();
    }

    [UnityTest]
    public IEnumerator ShouldStopPreviousTimers()
    {
        yield return new WaitForSeconds(.5f);

        _timers.Update();

        float previousTime = _timers.CurrentTime;

        _timers.ActiveIndex++;

        yield return new WaitForSeconds(.5f);

        _timers.List.TryGetValue(0, out float time);

        Assert.AreEqual(time, previousTime);
    }

    [TearDown]
    public void TearDown()
    { }
}