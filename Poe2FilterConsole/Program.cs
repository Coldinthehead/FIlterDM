using FilterCore;
using FilterCore.Conditions.Builders;
using FilterCore.Conditions.Impl;
using FilterCore.Decorations;
using FilterCore.Decorations.Builder;
using FilterCore.Enums;
using FilterCore.Parser;
using FilterCore.PoeData;
using System.Drawing;
using System.Text.Json;

namespace Poe2FilterConsole
{
    internal class Program
    {
        static Color HALF_TRANSPARENT = Color.FromArgb(125 , 255, 255, 255);

        static void Main(string[] args)
        {
            var input = @"
#===============================================================================================================
# NeverSink's Indepth Loot Filter - for Path of Exile
#===============================================================================================================
# VERSION:  0.1.1
# AUTHOR:   NeverSink
#
# This is a mini-filter designed for early PoE2 EA gameplay. I will eventually replace it with a full-fledged filter
# With FilterBlade.xyz support. This filter focuses more on highlighting loot, rather than hiding
#
# TWITTER: @NeverSinkDev
# DISCORD: https://discord.gg/mye6xhF
# TWITCH:  https://www.twitch.tv/neversink
# PATREON: https://www.patreon.com/Neversink

#--------------------------
# Overrides - Uniques, Valuables
#--------------------------

Show # Quaterstufersa
	BaseType ""Expert Slicing Quarterstaff"" ""Expert Barrier Quarterstaff"" ""Expert Long Quarterstaff"" ""Expert Gothic Quarterstaff""
	SetFontSize 45
	SetBorderColor 163 75 144 255 

Show # amour
	BaseType ""Expert Spiral Wraps"" ""Expert Linen Wraps"" ""Expert Gauze Wraps""  
	SetFontSize 45
	SetBorderColor 163 75 144 255 
	
Show #boots
	BaseType ""Expert Hunting Shoes"" ""Expert Threaded Shoes"" ""Expert Frayed Shoes""
	SetFontSize 45
	SetBorderColor 163 75 144 255 
	
Show #body
	BaseType  ""Expert Wayfarer Jacket"" ""Expert Scalper's Jacket"" ""Expert Waxed Jacket""
	SetFontSize 45
	SetBorderColor 163 75 144 255 
	
Show # helm
	BaseType ""Expert Feathered Tiara"" ""Expert Chain Tiara"" ""Expert Wicker Tiara""
	SetFontSize 45
	SetBorderColor 163 75 144 255 
	

Show
Rarity Unique
SetTextColor 175 96 37 255
SetBorderColor 175 96 37 255
SetBackgroundColor 53 13 13 255
PlayAlertSound 3 300
PlayEffect Brown
MinimapIcon 2 Brown Star
SetFontSize 40

## Divine Orb Style
Show
Class ""Currency""
BaseType ""Mirror"" ""Divine"" ""Perfect Jeweller's Orb"" ""Exalted Orb""
SetFontSize 45
SetTextColor 255 0 0 255
SetBorderColor 255 0 0 255
SetBackgroundColor 255 255 255 255
PlayAlertSound 6 300
PlayEffect Red
MinimapIcon 0 Red Star

#--------------------------
# Gold
#--------------------------

Show
StackSize >= 500
BaseType == ""Gold""
SetTextColor 255 255 255
SetBorderColor 255 255 255
PlayEffect Orange Temp

Show
BaseType == ""Gold""
SetTextColor 180 180 180
SetBorderColor 0 0 0 255
SetBackgroundColor 0 0 0 180

#--------------------------
# Uncut Gems
#--------------------------

Show
BaseType ""Uncut ""
SetTextColor 20 240 240
SetBorderColor 20 240 240
PlayAlertSound 2 300
PlayEffect Cyan
MinimapIcon 1 Cyan Triangle

#--------------------------
# Socketables and Special Character Equipment
#--------------------------

# Special A Tier - League specific socketables and jewels
Show 
BaseType ""Soul Core"" ""Timeless""
SetTextColor 0 240 190
SetBorderColor 0 240 190
SetFontSize 40
MinimapIcon 2 Cyan Triangle
PlayEffect Cyan

# Special A Tier - Sanctum Relics
Show 
Class ""Relic""
SetTextColor 0 240 190
SetBorderColor 0 240 190
SetFontSize 40
MinimapIcon 2 Cyan Triangle
PlayEffect Cyan

# Special A Tier - Rare Jewels
Show
Class ""Jewel""
Rarity Rare
SetTextColor 0 240 190
SetBorderColor 0 240 190
SetFontSize 40
MinimapIcon 2 Cyan Triangle
PlayEffect Cyan

# Special B Tier - Any Jewels
Show
Class ""Jewel""
Rarity Magic
SetTextColor 0 240 190
SetBorderColor 0 240 190
SetFontSize 40
PlayEffect Cyan Temp

# Special B Tier - Any Runes and Charms
Show 
BaseType "" Rune"" "" Charm""
SetTextColor 0 240 190
PlayEffect Cyan Temp

#--------------------------
# Socketables and Special Character Equipment
#--------------------------

# Currency Tier A: Gemcutter, Annullment
Show
Class ""Currency""
BaseType ""Gemcutter's Prism"" ""Orb of Annulment"" ""Orb of Chance""
SetTextColor 255 255 255 255
SetBorderColor 255 255 255 255
SetBackgroundColor 240 90 35
PlayAlertSound 1 300
PlayEffect White
MinimapIcon 1 White Circle
SetFontSize 40

# Currency Tier B: Vaal, Chaos, Exalt, Exotic
Show
Class ""Currency""
BaseType ""Vaal Orb"" ""Greater Jeweller's Orb"" ""Chaos Orb"" ""Lesser Jeweller's Orb"" ""Exotic"" ""Regal Orb"" ""Artificer's Orb"" ""Glassblower's Bauble"" ""Orb of Alchemy"" 
SetTextColor 255 207 132
SetBorderColor 255 207 132
SetBackgroundColor 76 51 12
PlayAlertSound 2 300
PlayEffect White
MinimapIcon 1 White Circle
SetFontSize 40

Show
Class ""Currency""
BaseType ""Distilled"" ""Catalyst"" ""Essence of"" ""Omen of""
SetTextColor 255 207 132
SetBorderColor 255 207 132
SetBackgroundColor 76 51 12
PlayAlertSound 2 300
PlayEffect White
MinimapIcon 1 White Circle

# Currency Tier C: Vaal, Chaos, Exalt, Exotic
Show
Class ""Currency""
BaseType ""Arcanist's Etcher"" ""Armourer's Scrap"" ""Blacksmith's Whetstone"" ""Orb of Augmentation"" ""Orb of Transmutation"" ""Regal Shard""
SetTextColor 255 207 132
SetBorderColor 255 207 132
MinimapIcon 2 Grey Circle

Show
Class ""Currency""
BaseType ""Simulacrum Splinter"" ""Breach Splinter"" "" Artifact""
SetTextColor 255 207 132
SetBorderColor 255 207 132
MinimapIcon 2 Grey Circle

Show
Class ""Currency""
BaseType ""Wisdom"" ""Shard""

# Unknown currency
Show
Class ""Currency""
SetTextColor 255 207 132
SetBorderColor 255 207 132
SetBackgroundColor 76 51 12
PlayAlertSound 2 300
PlayEffect Pink
MinimapIcon 1 White Circle

# Fragments
Show
BaseType ""Simulacrum"" "" Tablet"" ""Breachstone"" ""Barya"" ""Ultimatum"" "" Fragment"" ""Cowardly Fate"" ""Deadly Fate"" ""Victorious Fate""
SetTextColor 255 207 255
SetBorderColor 255 207 255
SetBackgroundColor 65 20 80
PlayAlertSound 2 300
PlayEffect White
MinimapIcon 1 White Square
SetFontSize 40

Show
BaseType ""Waystone""
Rarity <= Rare
SetTextColor 255 255 255
SetBorderColor 255 255 255
PlayAlertSound 4 300
PlayEffect White
MinimapIcon 1 White Square
SetFontSize 40

#--------------------------
# Value Rares
#--------------------------

Show
Class ""Rings"" ""Amulets"" ""Belts""
Rarity Rare
SetFontSize 40
SetTextColor 233 206 75
SetBorderColor 233 206 75
PlayEffect Yellow
MinimapIcon 1 Yellow Diamond

#--------------------------
# Rings, Amulets, Belts
#--------------------------

Show
Rarity Normal
Class ""Rings"" ""Amulets"" ""Belts""
SetFontSize 40

Show
Class ""Rings"" ""Amulets"" ""Belts""
Rarity Magic
SetFontSize 40

Hide
	Rarity Normal

#--------------------------
# Salvagable Items
#--------------------------

# Not working!
Show
Sockets > 0
Rarity Normal
SetBorderColor 200 200 200
SetFontSize 35

Show
Quality > 10
Rarity Normal
SetBorderColor 200 200 200
SetFontSize 35

# Not working!
Show
Sockets > 0
Rarity Magic
SetBorderColor 0 0 200
SetFontSize 35

Show
Quality > 10
Rarity Magic
SetBorderColor 0 0 200
SetFontSize 35

#--------------------------
# OPTIONAL RULES
#--------------------------
# TO ENABLE RULES, REMOVE THE # AT THE START OF THE LINE

### OPTIONAL RULE: Hide random bases
# REMOVE THE BASES YOU --DO-- WANT TO SEE BEFORE SETTING TO HIDE

# Hide
# Rarity <= Magic
# Class ""Flask"" ""Body"" ""Helmet"" ""Boots"" ""Gloves"" ""Shields"" ""Quiver"" ""Mace"" ""Staff"" ""Quarter"" ""Bow"" ""Crossbow"" ""Wand"" ""Sceptre""
# AreaLevel >= 65

### OPTIONAL RULE: REDUCES BACKGROUND ON LOW LEVEL BASES

# Show
# Rarity <= Magic
# Class ""Flask"" ""Body"" ""Helmet"" ""Boots"" ""Gloves"" ""Shields"" ""Quiver"" ""Mace"" ""Staff"" ""Quarter"" ""Bow"" ""Crossbow"" ""Wand"" ""Sceptre""
# AreaLevel >= 65
# DropLevel <= 50
# SetBackgroundColor 0 0 0 125

#--------------------------
# Meta
#--------------------------

# If this thing crashes, time to update your filter!
# This is here to prevent people to use this filter for AGES. Expert items will get removed eventually
# Get a new filter from www.filterblade.xyz

Show
BaseType == ""Expert Laced Boots""

	
    ";
            var lexer = new FilterLexer();
            var parser = new RuleParser();

            var tokens = lexer.BuildTokens(input);

            var rules = parser.Parse(tokens);
            foreach (var rule in rules )
            {
                Console.WriteLine($"{rule.StartToken} ");
                foreach (var node in rule.Nodes)
                {
                    Console.WriteLine($"\t{node.Operator}");
                    foreach (var pn in node.Parameters)
                    {
                        Console.WriteLine($"\t\t{pn}");
                    }
                }
            }
        }

        static void LoadItemClassRepository(string path)
        {
            var str = File.ReadAllText(path);
            List<ItemClassDetails> items = JsonSerializer.Deserialize<List<ItemClassDetails>>(str);
            foreach (var i in items)
            {
                Console.WriteLine(i.Name);
            }
        }

        static void BuildTest()
        {
            ParentBlock baseCurrencyBlock = BuildCurrency();
            ParentBlock expedition = BuildExpedition();
            ParentBlock gold = BuildGoldBlock();
            ParentBlock deli = BuildDeli();
            ParentBlock essence = BuildEssence();


            CheckCurrencyMatch([baseCurrencyBlock, expedition, gold, deli, essence], BaseItems());
            Console.WriteLine(baseCurrencyBlock.DumpString());
            Console.WriteLine(expedition.DumpString());
            Console.WriteLine(gold.DumpString());
            Console.WriteLine(deli.DumpString());
            Console.WriteLine(essence.DumpString());
            // dump fall back
            DumpFallback();
        }
        
        static ParentBlock BuildEssence()
        {
            ItemBlock allEssences = new ItemBlock("all");
            allEssences.AddDecoration(DecorationBuilder.GenericTierOne());
            allEssences.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Essence of the Body")
                .Include("Essence of the Mind")
                .Include("Essence of Enhancement")
                .Include("Essence of Torment")
                .Include("Essence of Flames")
                .Include("Essence of Ice")
                .Include("Essence of Electricity")
                .Include("Essence of Ruin")
                .Include("Essence of Battle")
                .Include("Essence of Sorcery")
                .Include("Essence of Haste")
                .Include("Essence of the Infinite")
                .Include("Greater Essence of the Body")
                .Include("Greater Essence of the Mind")
                .Include("Greater Essence of Enhancement")
                .Include("Greater Essence of Torment")
                .Include("Greater Essence of Flames")
                .Include("Greater Essence of Ice")
                .Include("Greater Essence of Electricity")
                .Include("Greater Essence of Ruin")
                .Include("Greater Essence of Battle")
                .Include("Greater Essence of Sorcery")
                .Include("Greater Essence of the Infinite")
                .Include("Greater Essence of Haste"));

            ParentBlock parent = new("Essence");
            parent.AddChildBlock(allEssences);

            return parent;
        }

        static ParentBlock BuildDeli()
        {
            ItemBlock distilled = new ItemBlock("Distilled currency");
            distilled.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Distilled Despair")
                .Include("Distilled Disgust")
                .Include("Distilled Envy")
                .Include("Distilled Fear")
                .Include("Distilled Greed")
                .Include("Distilled Guilt")
                .Include("Distilled Ire")
                .Include("Distilled Isolation")
                .Include("Distilled Paranoia")
                .Include("Distilled Suffering"));
            distilled.AddDecoration(DecorationBuilder.GenericTierOne());

            ItemBlock simulacrum = new ItemBlock("Simulacrum");
            simulacrum.FilterBy(ConditionBuilder.Start().FilterByTypes("Simulacrum"));
            simulacrum.AddDecoration(DecorationBuilder.GenericTierZero());

            ItemBlock splinters = new ItemBlock("Splinters");
            splinters.FilterBy(ConditionBuilder.Start().FilterByTypes("Simulacrum Splinter"));
            splinters.AddDecoration(DecorationBuilder.Splinter());

            ParentBlock deli = new("Delirium");
            deli.AddChildBlock(distilled);
            deli.AddChildBlock(simulacrum);
            deli.AddChildBlock(splinters);

            return deli;
        }

        static ParentBlock BuildGoldBlock()
        {
            ItemBlock largeGold = new("Large gold");
            largeGold.FilterBy(ConditionBuilder.Start()
                .FilterByTypes("Gold")
                .And()
                .FilterByStackSize(500));
            largeGold.AddDecoration(DecorationBuilder.Start()
                .WithSize(45)
                .WithPrimaryColor(Color.Red)
                .WithBackground(Color.Transparent));

            ItemBlock mediumGold = new ItemBlock("Medium gold");
            mediumGold.FilterBy(ConditionBuilder.Start()
                .FilterByTypes("Gold")
                .And()
                .FilterByStackSize(250));
            mediumGold.AddDecoration(DecorationBuilder.Start()
                .WithSize(40)
                .WithPrimaryColor(Color.Green)
                .WithBackground(Color.Transparent));

            ItemBlock smallGold = new("Small Gold");
            smallGold.FilterBy(ConditionBuilder.Start().FilterByTypes("Gold"));
            smallGold.AddDecoration(DecorationBuilder.Start()
                .WithSize(35)
                .WithPrimaryColor(Color.Yellow)
                .WithBackground(Color.Transparent));


            ParentBlock goldBlock = new("Gold");
            goldBlock.AddChildBlock(largeGold);
            goldBlock.AddChildBlock(mediumGold);
            goldBlock.AddChildBlock(smallGold);
            return goldBlock;
        }

        static ParentBlock BuildExpedition()
        {
            ItemBlock rerollCurrency = new ItemBlock("Reroll currency");
            rerollCurrency.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Exotic Coinage"));
            rerollCurrency.AddDecoration(DecorationBuilder.GenericTierOne());

            ItemBlock artifacts = new ItemBlock("Artifacts");
            artifacts.AddDecoration(DecorationBuilder.GenericTierOne());
            artifacts.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Broken Circle Artifact")
                .Include("Black Scythe Artifact")
                .Include("Order Artifact")
                .Include("Sun Artifact"));

            ItemBlock logbooks = new("Logbooks");
            logbooks.AddDecoration(DecorationBuilder.GenericTierOne());
            logbooks.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Expedition Logbook"));

            ParentBlock block = new("Expedition");
            block.AddChildBlock(rerollCurrency);
            block.AddChildBlock(artifacts);
            block.AddChildBlock(logbooks);

            return block;
        }

        static List<string> BaseItems()
        {
            string path = "./PoeData/BaseItems.txt";
            return File.ReadAllText(path).Split('\n').ToList();
        }

        static ParentBlock BuildCurrency()
        {
            ItemBlock tier0 = new ItemBlock("Currency T0");
            tier0.FilterBy(ConditionBuilder.Start()
                    .FilterByTypes()
                    .Include("Mirror of Kalandra")
                    .Include("Divine Orb")
                    .Include("Albino Rhoa Feather")
                    //  .Include("Orb of Transmutation")
                    .Include("Perfect Jeweller's Orb"));

            tier0.AddDecoration(
                DecorationBuilder.GenericTierZero());

            ItemBlock tier1 = new ItemBlock("Currency T1");
            tier1.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Chaos Orb")
                .Include("Exalted Orb")
                .Include("Orb of Alchemy")
                .Include("Orb of Chance")
                .Include("Regal Orb")
                .Include("Gemcutter's Prism")
                .Include("Glassblower's Bauble")
                .Include("Greater Jeweller's Orb")
                .Include("Artificer's Orb")
                .Include("Vaal Orb")
                .Include("Orb of Annulment"));

            tier1.AddDecoration(DecorationBuilder.GenericTierOne());



            ItemBlock tier2 = new ItemBlock("Currency T2");
            tier2.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Blacksmith's Whetstone")
                .Include("Arcanist's Etcher")
                .Include("Armourer's Scrap")
                .Include("Orb of Transmutation")
                .Include("Orb of Augmentation")
                .Include("Lesser Jeweller's Orb"));

            tier2.AddDecoration(DecorationBuilder.Start()
                .WithSize(40)
                .WithPrimaryColor(Color.Blue)
                .WithIcon()
                .WithColor(StaticGameColor.Blue)
                .WithShape(MinimapIconShape.Circle)
                .WithSize(MinimapIconSize.Small));

            ItemBlock tier3 = new ItemBlock("Currency T3");
            tier3.FilterBy(ConditionBuilder.Start()
                .FilterByTypes()
                .Include("Alchemy Shard")
                .Include("Chance Shard")
                .Include("Artificer's Shard")
                .Include("Transmutation Shard")
                .Include("Regal Shard"));

            tier3.AddDecoration(DecorationBuilder.Start()
                .WithSize(37)
                .WithPrimaryColor(Color.Black)
                .WithBackground(HALF_TRANSPARENT));

            ParentBlock baseCurrencyBlock = new("Base Currency");
            baseCurrencyBlock
                .AddChildBlock(tier0)
                .AddChildBlock(tier1)
                .AddChildBlock(tier2)
                .AddChildBlock(tier3);



            return baseCurrencyBlock;
        }

        static void DumpFallback()
        {
            Console.WriteLine("Show # fall back");
            Console.WriteLine(Helper.Tab + DecorationProperty.FontSize(45).Content);
            Console.WriteLine(Helper.Tab + DecorationProperty.BackgroundColor(Color.Purple).Content);
            Console.WriteLine(Helper.Tab + DecorationProperty
                .MinimapIcon(MinimapIconSize.Large, StaticGameColor.Purple, MinimapIconShape.Diamond).Content);
        }

        static void CheckCurrencyMatch(List<ParentBlock> blocks, List<string> declaredCurrencyItems)
        {
            Dictionary<string, int> currencyMap = [];
            foreach(string i in declaredCurrencyItems)
            {
                currencyMap.Add(i.TrimEnd(), 0);
            }
            var completedItems = blocks
                .SelectMany( x=> x.ChildBlocks)
                .Where(x=> x.GetFilteringCondition<BaseTypeCondition>() != null)
                .SelectMany(x => x.GetFilteringCondition<BaseTypeCondition>().BaseTypes)
                .ToList();
            foreach(string i in completedItems)
            {
                currencyMap[i] += 1;
            }
            Console.WriteLine(new string('=', 25));
            Console.WriteLine($"completed : {completedItems.Count} / {currencyMap.Count}");
            var nonCompleted = currencyMap.Where( (x, y) => x.Value == 0).Select( x=> x.Key).ToList();
            foreach (string i in nonCompleted)
            {
                Console.WriteLine(">" + i);
            }
            Console.WriteLine(new string('=', 25));
            Console.WriteLine(new string('=', 25));
            var multipleInstance = currencyMap.Where((x, y) => x.Value > 1).Select(x => x.Key).ToList();
            Console.WriteLine("Multiple Enties : ");
            foreach (string i in multipleInstance)
            {
                List<string> selectedBlocks 
                    = blocks
                    .SelectMany(x=> x.ChildBlocks)
                    .Where(x=> x.GetFilteringCondition<BaseTypeCondition>() != null)
                    .Select(x=> x.GetFilteringCondition<BaseTypeCondition>())
                    .Where(x => x.BaseTypes.Contains(i)).Select(x => x.ParentName).ToList();
                string saperatedNames = string.Join('|', selectedBlocks);
                Console.WriteLine($"{i} in : {saperatedNames}");
            }
            Console.WriteLine(new string('=', 25));
        }
    }
}
