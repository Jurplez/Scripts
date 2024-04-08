/*
name: 0MaxDarkonMats
description: Max all of the Darkon materials
tags: darkon, max, all, new class
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Darkon/CoreDarkon.cs
//cs_include Scripts/Story/ElegyofMadness(Darkon)/CoreAstravia.cs
using Skua.Core.Interfaces;

public class MaxAllDarkon
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreDarkon Darkon => new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        Darkon.AMelody(MaxStack: true);
        Darkon.AncientRemnant(MaxStack: true);
        Darkon.BanditsCorrespondence(MaxStack: true);
        Darkon.FarmReceipt(MaxStack: true);
        Darkon.LasGratitude(MaxStack: true);
        Darkon.WheelofFortune(MaxStack: true);
        Darkon.SukisPrestiege(MaxStack: true);
        Darkon.Teeth(MaxStack: true);
        Darkon.UnfinishedMusicalScore(300);

        Core.SetOptions(false);
    }
}