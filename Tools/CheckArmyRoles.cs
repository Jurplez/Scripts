/*
name: Check Roles
description: This script will give a popup telling you a bunch of information regarding your account.
tags: tool, evaluate, account, chrono, heromart, beta, founder, badges, enhancements, rare, seasonal
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using System.Text;

public class CheckArmyRoles
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    public CoreAdvanced Adv = new();
    private CoreStory Story = new();

    public void ScriptMain(IScriptInterface Bot)
    {
        Evaluate();
    }

    public void Evaluate()
    {
        while (!Bot.ShouldExit && !Bot.Player.Loaded)
            Core.Sleep();

        Core.Join("Battleon-999999");

        Bot.Bank.Open();

        // Load Forge Enhancement Quest + some role quests
        Bot.Quests.Load(forgeEnhIDs.Concat(new[] { 793, 2937, 8042 }).ToArray());
        Bot.Wait.ForTrue(() => Bot.Bank.Items.Any(), 20);

        int dmgAll51Items = 0;
        (RacialGearBoost, bool)[] racial75Items = racialGears.Select(x => (x, false)).ToArray();

        Core.Logger("🗂️ Checking Player Inventory Items...");
        processItems("world.myAvatar.items");

        Core.Logger("🏦 Reviewing Player Bank Items...");
        processItems("world.bankinfo.items");

        Core.Logger("🏠 Evaluating Player House Items...");
        processItems("world.myAvatar.houseitems");

        Core.Logger("📝 Compiling and formatting all data for the final output...");

        #region OutPut Generator
        // The actual output
        Bot.ShowMessageBox(
                        $"Evaluation ID: {GenerateEvaluationID()}\n" +
                        $"Extra Secutity: Account Item Count: {Bot.Inventory.Items.Concat(Bot.Bank.Items).Concat(Bot.House.Items).Count()}\n" +
                        $"Ｅｎｈａｎｃｅｍｅｎｔｓ\n" +
                        $"(Victor of War) Valiance:\t\t\t\t{Checkbox(Core.isCompletedBefore(8741))}\n" +
                        $"(Conductor of War) Arcana's Concerto:\t\t{Checkbox(Core.isCompletedBefore(8742))}\n" +
                        $"(Deliverance of War) Elysium:\t\t\t{Checkbox(Core.isCompletedBefore(8821))}\n" +
                        $"(Reflectionist of War) Examen:\t\t\t{Checkbox(Core.isCompletedBefore(8825))}\n" +
                        $"(Penitent of War) Pentience:\t\t\t{Checkbox(Core.isCompletedBefore(8822))}\n" +
                        $"(Miltonious of War) Ravenous:\t\t\t{Checkbox(Core.isCompletedBefore(9560))}\n" +
                        $"(Shadow of War) Dauntless:\t\t\t{Checkbox(Core.isCompletedBefore(9172))}\n" +

                        $"Ｃｌａｓｓｅｓ\n" +
                        "(Avenger of War) " + importantItemCheckbox(3, "Chaos Avenger") +
                        "(ArchMage of War) " + importantItemCheckbox(3, "ArchMage") +
                        "(Revenant of War) " + importantItemCheckbox(3, "Legion Revenant") +
                        "(Highlord of War) " + importantItemCheckbox(3, "Void Highlord", "Void Highlord (IoDA)") +
                        "(Vera of War) " + importantItemCheckbox(3, "Verus DoomKnight") +
                        "(Eternal Dragon of War) " + importantItemCheckbox(2, "Dragon of Time") +
                        "(Diviner of War) " + importantItemCheckbox(3, "Arcana Invoker") +
                        "(Tempest of War) " + importantItemCheckbox(2, "Sovereign of Storms") +

                        $"Ｗｅａｐｏｎｓ\n" +
                        "(Prisoner of War) " + importantItemCheckbox(1, "Hollowborn Reaper's Scythe") +
                        "(Primordial of War) " + importantItemCheckbox(2, "Necrotic Sword of Doom") +
                        "(Wraith of War) " + importantItemCheckbox(2, "Hollowborn Sword of Doom") +
                        "(Legatus of War) " + importantItemCheckbox(1, "Necrotic Blade of the Underworld") +
                        "(Chauvinist of War) " + importantItemCheckbox(1, "Necrotic Sword of the Abyss") +
                        "(Prudence of War) " + importantItemCheckbox(3, "Providence") +
                        "(Sinner of War) " + importantItemCheckbox(3, "Sin of the Abyss") +
                        "(Deacon of War) " + importantItemCheckbox(2, "Exalted Apotheosis") +
                        "(Deacon of War) " + importantItemCheckbox(2, "Dual Exalted Apotheosis") +
                        "(Celestial of War) " + importantItemCheckbox(1, "Greatblade of the Entwined Eclipse") +
                        "(Starlight of War) " + importantItemCheckbox(2, "Star Light of the Empyrean", "Star Lights of the Empyrean") +

                        $"Ａｒｍｏｒ\n" +
                        $"(Ascendant of War) Awescended:\t\t\t{Checkbox(Core.isCompletedBefore(8042))}\n" +
                        "(Radiant Goddess of War) " + importantItemCheckbox(1, "Radiant Goddess of War") +

                        $"Ｃｈｒｏｎｏ Ｃｈｅｃｋ\n" +
                        $"(Time Lord of War) Chronomancer\t\t\t{Checkbox(ChronoOwned())}\n" +


                        $"Ｅｎｄ Ｃｈｅｃｋｓ\n" +
                        $"Apprentice of War:\t\t\t\t{Checkbox(ApprenticeOfWar())}\n" +
                        $"Master of War:\t\t\t\t\t{Checkbox(MasterofWar())}\n" +
                        $"Apostle of War:\t\t\t\t\t{Checkbox(Apostleofwar())}\n" +
                        $"Bishop of War:\t\t\t\t\t{Checkbox(BishopofWar())}\n" +
                        $"Cardinal of War:\t\t\t\t\t{Checkbox(CardinalofWar())}\n" +
                        $"51% DMG All Weapons:\t\t\t\t{dmgAll51Items} out of 30\n\n" +

                        "\n" +
                        "\n" +

                        $"Ｉｎｆｏ  Ｆｏｒ  Ｐｌａｙｅｒ\n" +
                        GetStatusReport(),

                        "Evaluation Complete"
                    );

        Core.Logger("✅ All processes complete! Ready for final review.");


        #endregion

        void processItems(string prop)
        {
            var list = Bot.Flash.GetGameObject<List<dynamic>>(prop);
            if (list != null)
            {
                for (int i = 0; i < racial75Items.Length; i++)
                {
                    if (!racial75Items[i].Item2 &&
                        list.Any(item =>
                            item.sMeta != null &&
                            ((string)item.sMeta).Contains($"{racial75Items[i].Item1}:1.75")))
                        racial75Items[i].Item2 = true;
                }
                dmgAll51Items += list.Count(item => item.sMeta != null && ((string)item.sMeta).Contains("dmgAll:1.51"));
            }
        }

    }

    private bool ApprenticeOfWar()
    {
        return Bot.Player.Level >= 100 &&
                Bot.Inventory.Items.Concat(Bot.Bank.Items).Any(item => dpsClasses.Contains(item.Name)) &&
                Bot.Inventory.Items.Concat(Bot.Bank.Items).Any(item => farmerClasses.Contains(item.Name)) &&
                Bot.Inventory.Items.Concat(Bot.Bank.Items).Any(item => supportClasses.Contains(item.Name));
    }

    private bool MasterofWar()
    {
        return ApprenticeOfWar()
               && Bot.Inventory.Items.Concat(Bot.Bank.Items).Any(item => item != null && Core.GetBoostFloat(item, "dmgAll") > 1.3f && !IsNonWeapon(item))
               && MasterofWarMeta();
    }

    private bool Apostleofwar()
    {
        return MasterofWar()
       && Core.CheckInventory(ApostleWeapons, any: true, toInv: false) || Core.CheckInventory(Apostleinsignias, toInv: false);
    }

    private bool BishopofWar()
    {
        int bishopClassesOwned = BishopRequirements
        .Take(7) // First 7 are Bishop classes
        .Count(cls => Bot.Inventory.Items
            .Concat(Bot.Bank.Items)
            .Any(item => item.Name == cls));

        int FiftyOneWeaponsOwned = Bot.Inventory.Items
                    .Concat(Bot.Bank.Items)
                    .Count(item => item != null && FiftyOneWeapons.Contains(item.Name));

        return Apostleofwar()
            // Check for 51% damage boost weapon
            && Bot.Inventory.Items.Concat(Bot.Bank.Items).Any(item => item != null && Core.GetBoostFloat(item, "dmgAll") >= 1.5f)
            // Check for Bishop Data Classes (index 0 to 6)
            && Core.CheckInventory(BishopRequirements[0..7], any: true, toInv: false)
            // Check for Nulgath insignias or items (index 7 to 9)
            && Core.CheckInventory(BishopRequirements[7..10], any: true, toInv: false)
            // Check for Dage insignias or items (index 10 to end)
            && Core.CheckInventory(BishopRequirements[10..], any: true, toInv: false)
            // Check for >= 1 of bishopClassesOwned && FiftyOneWeaponsOwned
            && bishopClassesOwned >= 1 && FiftyOneWeaponsOwned >= 1;
    }

    private bool CardinalofWar()
    {
        // IDs for enhancements
        int[] weaponEnhancements = { 8738, 8739, 8740, 8741, 8742, 8758, 8821, 8820, 9560, 8744 };
        int[] helmEnhancements = { 8826, 8825, 8758, 8827, 8824 };
        int[] capeEnhancements = { 8743, 8745, 8758, 8823, 8822, 8744 };

        // Check for at least one unlocked enhancement in each category
        bool hasEnhancements = new[]
        {
        weaponEnhancements.Any(Core.isCompletedBefore),
        helmEnhancements.Any(Core.isCompletedBefore),
        capeEnhancements.Any(Core.isCompletedBefore)
        }.All(check => check);

        int FiftyOneWeaponsOwned = Bot.Inventory.Items
            .Concat(Bot.Bank.Items)
            .Count(item => item != null && FiftyOneWeapons.Contains(item.Name));

        int bishopClassesOwned = BishopRequirements
               .Take(7) // First 7 are Bishop classes
               .Count(cls => Bot.Inventory.Items
                   .Concat(Bot.Bank.Items)
                   .Any(item => item.Name == cls));

        // Return true if at least 4 Bishop classes are owned and all enhancements are unlocked *and* has the nightmare Carnax Boss badge
        return BishopofWar() && bishopClassesOwned >= 4 && hasEnhancements && Core.isCompletedBefore(8873) && FiftyOneWeaponsOwned >= 4;
    }
    #region Variables
    // DPS Classes
    string[] dpsClasses = new[]
    {
        "Dragon of Time",
        "Glacial Berserker",
        "Guardian",
        "Legion DoomKnight",
        "Legion Revenant",
        "LightCaster",
        "Lycan",
        "Psionic MindBreaker",
        "Void HighLord"
};
    // Farmers
    string[] farmerClasses = new[]
    {
        "Abyssal Angel",
        "ArchMage",
        "Blaze Binder",
        "Daimon",
        "Dragon of Time",
        "Eternal Inversionist",
        "Firelord Summoner",
        "Legion Revenant",
        "Dark Master of Moglins",
        "Master of Moglins",
        "NCM",
        "Scarlet Sorceress",
        "ShadowScythe General",
        "Shaman"
    };
    // Support Classes
    string[] supportClasses = new[]
    {
        "ArchFiend",
        "ArchPaladin",
        "Frostval Barbarian",
        "Infinity Titan",
        "Dark Legendary Hero",
        "Legendary Hero",
        "Legion Revenant",
        "LightCaster",
        "Lord of Order",
        "NorthLands Monk",
        "Quantum Chronomancer",
        "Continuum Chronomancer",
        "StoneCrusher"
    };
    private int[] rareIDs =
    {
        21, // Limited Time Drop
        68, // New Collection Chest
        35, // Rare
        40, // Import Item
        55, // Sesaonal Rare
        60, // Event Item
        65, // Event Rare
        70, // Limited Rare
        75, // Collector's Rare
        80, // Promotional Item
        90, // Ultra Rare
        95, // Super Mega Ultra Rare
    };
    private string[] houseCat =
    {
        "Floor Item",
        "Wall Item",
        "House",
    };
    private int[] forgeEnhIDs =
    {
        8738,
        8739,
        8740,
        8741,
        8742,
        8743,
        8745,
        8758,
        8821,
        8820,
        8822,
        8823,
        8824,
        8825,
        8826,
        8827,
        9172,
        9171,
    };
    private (string Name, int ID)[] ForgeQuests = new (string Name, int ID)[]
      {
    ("Forge Weapon Enhancement", 8738),
    ("Lacerate", 8739),
    ("Smite", 8740),
    ("Hero's Valiance", 8741),
    ("Arcana's Concerto", 8742),
    ("Absolution", 8743),
    ("Avarice", 8745),
    ("Praxis", 9171),
    ("Acheron", 8820),
    ("Elysium", 8821),
    ("Penitence", 8822),
    ("Lament", 8823),
    ("Vim, Ether", 8824),
    ("Anima", 8826),
    ("Pneuma", 8827),
    ("Dauntless", 9172),
    ("Forge Cape Enhancement", 8758),
    ("Examen", 8825)
      };
    private string[] HeroMartClasses =
    {
        "CardClasher",
        "Chrono Chaorruptor",
        "Chrono Commandant",
        "Chrono DataKnight",
        "Chrono DragonKnight",
        "ChronoCommander",
        "ChronoCorruptor",
        "Chronomancer",
        "Chronomancer Prime",
        "Classic Defender",
        "Classic Dragonlord",
        "Classic Guardian",
        "Continuum Chronomancer",
        "Corrupted Chronomancer",
        "Dark Master of Moglins",
        "Defender",
        "Dragonlord",
        "DoomKnight OverLord",
        "Dragon Knight",
        "Empyrean Chronomancer",
        "Eternal Chronomancer",
        "Flame Dragon Warrior",
        "Great Thief",
        "Guardian",
        "Heavy Metal Rockstar",
        "Heavy Metal Necro",
        "Immortal Chronomancer",
        "Infinity Knight",
        "Interstellar Knight",
        "Legion Paladin",
        "Master of Moglins",
        "Nechronomancer",
        "Necrotic Chronomancer",
        "NOT A MOD",
        "Nu Metal Necro",
        "Obsidian Paladin Chronomancer",
        "Overworld Chronomancer",
        "Paladin Chronomancer",
        "Paladin Highlord",
        "PaladinSlayer",
        "Quantum Chronomancer",
        "ShadowStalker of Time",
        "ShadowWalker of Time",
        "ShadowWeaver of Time",
        "Star Captain",
        "StarLord",
        "TimeKeeper",
        "TimeKiller",
        "Timeless Chronomancer",
        "Underworld Chronomancer",
        "Unchained Rocker",
    };
    string[] ApostleWeapons = new[]
    {
        "Exalted Penultima",
        "Exalted Unity",
        "Exalted Apotheosis"
    };
    string[] Apostleinsignias = new[]
    {
        "Ezrajal Insignia",
        "Warden Insignia",
        "Engineer Insignia"
    };
    string[] BishopRequirements = new[]
    {
    // Bishop Classes
    "Chaos Avenger",
    "ArchMage",
    "Legion Revenant",
    "Void HighLord",
    "Dragon of Time",
    "Verus DoomKnight",
    "Arcana Invoker",

    // Nulgath Insignias and Items
    "Nulgath Insignia",
    "Sin of the Abyss",
    "Sin of Revontheus",

    // Dage Insignias and Items
    "Dage the Evil Insignia",
    "Necrotic Blade of the Underworld"
    };
    string[] FiftyOneWeapons = new[]
    {
        "Necrotic Sword of Doom",
        "Hollowborn Sword of Doom",
        "Necrotic Blade of the Underworld",
        "Necrotic Sword of the Abyss",
        "Providence",
        "Sin of the Abyss",
        "Exalted Apotheosis",
        "Dual Exalted Apotheosis",
        "Greatblade of the Entwined Eclipse",
        "Star Light of the Empyrean",
        "Star Lights of the Empyrean"
    };
    #endregion

    #region Methods
    /// <summary>
    /// Generates a string representation of an item with a checkbox indicating its presence in the inventory.
    /// </summary>
    /// <param name="tabs">The number of tabs to include before the item name. Default is 0.</param>
    /// <param name="items">The item names to check in the inventory. The first item is used for the checkbox label.</param>
    /// <returns>A string with the item name, tabs, and checkbox status (🗸 for present, X for absent).</returns>
    string importantItemCheckbox(int tabs = 0, params string[] items)
    {
        // Generate the required number of tabs
        string _tabs = new('\t', tabs);

        // Determine if the items exist in the inventory
        bool check = Core.CheckInventory(items, 1, true, false);

        // Return the item name, tabs, and its checkbox status (🗸 for true, X for false)
        return $"{items[0]}:{_tabs}{Checkbox(check)}\n";
    }
    /// <summary>
    /// Returns a checkbox representation based on a boolean value.
    /// </summary>
    /// <param name="check">The boolean value indicating whether the checkbox should be checked.</param>
    /// <returns>A string representation of a checkbox with a checkmark (🗸) or an X (X) depending on the value of <paramref name="check"/>.</returns>
    string Checkbox(bool check) =>
     $"[ {(check ? "✅" : "❌")} ]";
    /// <summary>
    /// Filters out items that are neither weapons nor armor from the inventory.
    /// </summary>
    /// <param name="x">The inventory item to evaluate.</param>
    /// <returns>True if the item is neither a weapon nor armor, otherwise false.</returns>
    bool NoneEnhFilter(InventoryItem x)
    {
        return
         x.Category != ItemCategory.Sword
            && x.Category != ItemCategory.Axe
            && x.Category != ItemCategory.Dagger
            && x.Category != ItemCategory.Gun
            && x.Category != ItemCategory.HandGun
            && x.Category != ItemCategory.Rifle
            && x.Category != ItemCategory.Bow
            && x.Category != ItemCategory.Mace
            && x.Category != ItemCategory.Gauntlet
            && x.Category != ItemCategory.Polearm
            && x.Category != ItemCategory.Staff
            && x.Category != ItemCategory.Wand
            && x.Category != ItemCategory.Whip
            && x.Category != ItemCategory.Helm
            && x.Category != ItemCategory.Cape;
    }
    /// <summary>
    /// Checks the completion status of Forge quests and lists any that are incomplete.
    /// </summary>
    /// <param name="incompleteQuests">An output parameter that will contain the names and IDs of any incomplete quests.</param>
    /// <returns>The number of completed Forge quests.</returns>
    private int CheckForgeQuests(out List<string> incompleteQuests)
    {
        incompleteQuests = new List<string>();
        int unlockedQuests = 0;

        foreach (var (Name, ID) in ForgeQuests)
        {
            var quest = Bot.Quests.EnsureLoad(ID);
            if (quest != null)
            {
                if (Core.isCompletedBefore(quest.ID))
                {
                    unlockedQuests++;
                }
                else
                {
                    incompleteQuests.Add($"'{Name}' (ID: {ID}) is not completed.");
                }
            }
            else
            {
                incompleteQuests.Add($"Failed to load quest '{Name}' (ID: {ID}).");
            }
        }

        return unlockedQuests;
    }
    /// <summary>
    /// Checks if the player owns any class related to "Chrono" or "Time" 
    /// from either their inventory or bank.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if a "Chrono" or "Time" class is found, 
    /// otherwise returns <c>false</c>.
    /// </returns>
    private bool ChronoOwned()
    {
        // Filter for Chrono/Time classes
        string[] ChronoClasses = HeroMartClasses.Where(x => x.Contains("Chrono") || x.Contains("Time")).ToArray();

        // Check if any Chrono class is found in Inventory or Bank
        return Bot.Inventory.Items
            .Concat(Bot.Bank.Items)
            .Any(x => ChronoClasses.Contains(x.Name));
    }
    /// <summary>
    /// Generates a unique evaluation ID by combining a random alphanumeric string with a hexadecimal representation of the username, and scrambling the result.
    /// </summary>
    /// <returns>A scrambled evaluation ID string.</returns>
    private string GenerateEvaluationID()
    {
        // Alphanumeric characters
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Create Random object
        Random random = new();

        // Generate a random alphanumeric string
        string randomString = new(Enumerable.Range(0, 16)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());

        // Convert username to hexadecimal string
        string usernameHex = BitConverter.ToString(Encoding.UTF8.GetBytes(Core.Username()))
            .Replace("-", "");

        // Combine the random string with the hexadecimal string
        string combinedString = randomString + usernameHex;

        // Scramble the combined string
        return new string(combinedString
            .OrderBy(_ => random.Next())
            .ToArray());
    }
    /// <summary>
    /// Builds and returns a status report summarizing the completion of Forge quests and any additional relevant information.
    /// </summary>
    /// <returns>A formatted status report string.</returns>
    public string GetStatusReport()
    {
        // Call CheckForgeQuests to get the list of incomplete quests
        int unlockedQuests = CheckForgeQuests(out List<string> incompleteQuests);

        // Build the report string
        var reportBuilder = new System.Text.StringBuilder();
        reportBuilder.AppendLine($"Number of unlocked quests: {unlockedQuests}");

        if (incompleteQuests.Count > 0)
        {
            reportBuilder.AppendLine("Incomplete quests:");
            foreach (var quest in incompleteQuests)
            {
                reportBuilder.AppendLine(quest);
            }
        }
        else
        {
            reportBuilder.AppendLine("All Forge Quests are completed!");
        }
        return reportBuilder.ToString();
    }
    /// <summary>
    /// Gets the list of racial gear boosts excluding specific types.
    /// </summary>
    /// <returns>An array of racial gear boosts, excluding the specified types.</returns>
    private RacialGearBoost[] racialGears =>
        Enum.GetValues<RacialGearBoost>().Except(RacialGearBoost.None, RacialGearBoost.Drakath, RacialGearBoost.Orc);
    /// <summary>
    /// Determines if an inventory item is neither a weapon nor armor.
    /// </summary>
    /// <param name="x">The inventory item to evaluate.</param>
    /// <returns>True if the item is neither a weapon nor armor; otherwise, false.</returns>
    bool IsNonWeapon(InventoryItem x)
    {
        return x.Category != ItemCategory.Sword
            && x.Category != ItemCategory.Axe
            && x.Category != ItemCategory.Dagger
            && x.Category != ItemCategory.Gun
            && x.Category != ItemCategory.HandGun
            && x.Category != ItemCategory.Rifle
            && x.Category != ItemCategory.Bow
            && x.Category != ItemCategory.Mace
            && x.Category != ItemCategory.Gauntlet
            && x.Category != ItemCategory.Polearm
            && x.Category != ItemCategory.Staff
            && x.Category != ItemCategory.Wand
            && x.Category != ItemCategory.Whip;
    }
    public bool MasterofWarMeta()
    {
        // Define meta types to exclude
        var excludedMetaTypes = new[] { "Drakath", "AutoAdd" };

        // Collect all items from inventory and bank, filtering out weapon categories
        List<ItemBase> items = Bot.Inventory.Items.OfType<InventoryItem>()
            .Where(NoneEnhFilter)  // Apply non-weapon filter
            .Concat(Bot.Bank.Items.OfType<InventoryItem>().Where(NoneEnhFilter))
            .Cast<ItemBase>()
            .ToList();

        // Iterate through each item
        foreach (ItemBase item in items)
        {
            if (item?.Meta == null)
                continue;  // Skip items with no meta data

            // Count valid meta types in the item's meta data
            int validMetaCount = item.Meta
                .Split('\n')  // Split meta data into lines
                .SelectMany(line => line.Replace("AutoAdd,", string.Empty).Split(','))  // Remove "AutoAdd," and split by commas
                .Select(meta => meta.Split(':'))  // Split each meta entry into key and value
                .Count(metaPair => metaPair.Length == 2
                    && !excludedMetaTypes.Contains(metaPair[0])  // Exclude unwanted meta types
                    && double.TryParse(metaPair[1], out double value)  // Parse value as double
                    && value >= 1.3);  // Check if value is >= 1.3

            // Return true if at least 4 valid meta types are found
            if (validMetaCount >= 4)
                return true;
        }

        // Return false if no items meet the criteria
        return false;
    }




    #endregion
}