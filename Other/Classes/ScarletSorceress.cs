/*
name: ScarletSorceress
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Story/ThroneofDarkness/CoreToD.cs
//cs_include Scripts/Other/Classes/BloodSorceress.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;

public class ScarletSorceress
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm => new();
    public CoreAdvanced Adv = new();
    public CoreToD TOD = new();
    public BloodSorceress BS = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        GetSSorc();

        Core.SetOptions(false);
    }

    public void GetSSorc(bool rankUpClass = true)
    {
        if (Core.CheckInventory("Scarlet Sorceress"))
        {
            if (rankUpClass)
                Adv.RankUpClass("Scarlet Sorceress");
            return;
        }

        Core.AddDrop("Scarlet Sorceress");

        TOD.TowerofMirrors();
        BS.GetBSorc(false);

        InventoryItem? BloodSorceress = null;
        for (int i = 0; i < 5; i++)
        {
            BloodSorceress = Bot.Inventory.Items.Find(i => i.Name.ToLower().Trim() == "Blood Sorceress".ToLower().Trim() && i.Category == ItemCategory.Class);
            if (BloodSorceress != null)
                break;
            Core.Logger($"Attempt {i + 1}: Blood Sorceress not found in inventory. Retrying...");
            Core.Sleep(1000); // Wait for 1 second before retrying
        }

        if (BloodSorceress == null)
        {
            Core.Logger("Blood Sorceress not found in inventory after 5 attempts.");
            return;
        }

        InventoryItem? ScarletSorceress = null;
        for (int i = 0; i < 5; i++)
        {
            ScarletSorceress = Bot.Inventory.Items.Find(i => i.Name.ToLower().Trim() == "Scarlet Sorceress".ToLower().Trim() && i.Category == ItemCategory.Class);
            if (ScarletSorceress != null)
                break;
            Core.Logger($"Attempt {i + 1}: Scarlet Sorceress not found in inventory. Retrying...");
            Core.Sleep(1000); // Wait for 1 second before retrying
        }

        if (ScarletSorceress == null)
        {
            Core.Logger("Scarlet Sorceress not found in inventory after 5 attempts.");
            return;
        }

        // Requires rank 10 now, ensure this is the case.
        Adv.RankUpClass("Blood Sorceress");

        Core.JumpWait();

        if (BloodSorceress.Quantity < 302500) //now requires it to be rank 10?
        {
            Core.Relogin();
            Adv.RankUpClass(BloodSorceress.Name);
        }

        Farm.Experience(50);

        Core.ChainComplete(6236);
        Bot.Wait.ForPickup("Scarlet Sorceress");

        if (ScarletSorceress.EnhancementLevel == 0)
            Adv.SmartEnhance(ScarletSorceress.Name);

        if (rankUpClass)
            Adv.RankUpClass(ScarletSorceress.Name);
    }
}
