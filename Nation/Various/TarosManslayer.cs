//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Nation/CoreNation.cs
//cs_include Scripts/Nation/Various/PurifiedClaymoreOfDestiny.cs
using Skua.Core.Interfaces;
public class TarosManslayer
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new CoreFarms();
    public CoreNation Nation = new();
    public PurifiedClaymoreOfDestiny PCoD = new PurifiedClaymoreOfDestiny();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        GuardianTaro();

        Core.SetOptions(false);
    }

    private string[] Rewards = { "Taro's Manslayer", "Taro Blademaster Guardian" };

    public void GuardianTaro(bool ManslayerOnly = false)
    {
        if (ManslayerOnly)
        {
            Rewards = new[] { "Taro's Manslayer" };
            if (Core.CheckInventory(Rewards))
                return;

            Core.AddDrop(Rewards);

            Farm.GoodREP();
            PCoD.GetPCoD();

            if (ManslayerOnly & !Core.IsMember)
            {
                Core.HuntMonster("tercessuinotlim", "Taro Blademaster", "Taro's Manslayer", isTemp: false);
                return;
            }

            while (!Bot.ShouldExit && !Core.CheckInventory(Rewards))
            {
                Core.EnsureAccept(1111);
                Nation.FarmGemofNulgath(10);
                Core.HuntMonster("tercessuinotlim", "Dark Makai", "Dark Makai Rune");
                Core.EnsureCompleteChoose(1111, Rewards);
                Bot.Sleep(Core.ActionDelay);
            }
        }
    }
}