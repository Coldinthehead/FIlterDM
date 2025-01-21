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
