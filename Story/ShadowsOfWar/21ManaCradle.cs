/*
name: Mana Cradle
description: This will finish the Mana Cradle quests.
tags: story, quest, shadow-war, mana cradle,ultra speaker,ultraspeaker,malgor
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Story/ShadowsOfWar/CoreSoW.cs
using Skua.Core.Interfaces;

public class ManaCradle
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreSoW SoW = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        SoW.ManaCradle();

        Core.SetOptions(false);
    }
}
