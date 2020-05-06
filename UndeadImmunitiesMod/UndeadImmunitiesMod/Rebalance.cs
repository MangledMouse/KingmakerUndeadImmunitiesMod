using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Facts;
using System.Runtime;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Encyclopedia;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine;
using Harmony12;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;

namespace UndeadImmunitiesMod
{

    class Rebalance
    {
        //for those curious at home about how and why this happened
        //undead have two different immunity packages, one with all the mindaffecting pieces and one with many other immunities
        //the mind affacting immunity package is bypassed by the undead bloodline and similar abilities like Will of the Dead from Call of the Wild
        //However the second list includes many mind affecting categories a second time. This makes it so the undead retain their immunity
        //to those effects through the Undead Bloodline. Very odd. This fixes that
        static LibraryScriptableObject library => Main.library;
        static public void undeadImmunitiesChange()
        {

           

            BlueprintFeature undeadImmunities = library.Get<BlueprintFeature>("8a75eb16bfff86949a4ddcb3dd2f83ae");

            //Undead have 3 condition immunities, 1 for nauseated, 1 for fatigued and 1 for sickened

            //They also 2 base buff descriptor immunities, which in turn each grant many immunities
            //Buff descriptor immunity: Poison, Disease, Sickened, Shaken, Fatigue, Frightened, Exhausted, Confusion, Death, Bleed, VilderavnBleed
            //Buff descriptor immunity: MindAffecting, Fear, Compulsion, Emotion, Charm, Daze, Shaken, Frightened, Confusion, Sleep

            //Stun and sleep are added here because, by PnP rules, Undead are immune to these traits in addition to being immune to mind-affecting things
            //SpellDescriptor sd1 = (SpellDescriptor)((long)SpellDescriptor.Poison + (long)SpellDescriptor.Disease + (long)SpellDescriptor.Fatigue + (long)SpellDescriptor.Exhausted + (long)SpellDescriptor.Death + (long)SpellDescriptor.Bleed + (long)SpellDescriptor.VilderavnBleed) + (long)SpellDescriptor.Stun + (long)SpellDescriptor.Sleep;
            SpellDescriptor sd1 = (SpellDescriptor)((long)SpellDescriptor.Poison + (long)SpellDescriptor.Disease + (long)SpellDescriptor.Sickened +(long)SpellDescriptor.Fatigue + (long)SpellDescriptor.Exhausted + (long)SpellDescriptor.Death + (long)SpellDescriptor.Bleed + (long)SpellDescriptor.VilderavnBleed + (long)SpellDescriptor.Stun + (long)SpellDescriptor.Sleep);
            //The list to bypass now includes only mindaffecting elements. As the PnP rules do not specificy that Daze
            //is mind-affecting that has been removed from their immunities entirely
            //its possible that this should be the mindaffecting trait alone or all of the mind affecting things
            //SpellDescriptor sd2 = (SpellDescriptor)((long)SpellDescriptor.MindAffecting + (long)SpellDescriptor.Fear + (long)SpellDescriptor.Compulsion + (long)SpellDescriptor.Emotion + (long)SpellDescriptor.Charm + (long)SpellDescriptor.Shaken + (long)SpellDescriptor.Frightened + (long)SpellDescriptor.Confusion);
            SpellDescriptor sd2 = (SpellDescriptor)((long)SpellDescriptor.MindAffecting);
            foreach (BuffDescriptorImmunity bdi in undeadImmunities.GetComponents<BuffDescriptorImmunity>())
            {
                if((long)bdi.Descriptor.Value == 6597687263488) //This is the first buff descriptor's value which includes redundant buffs
                {                    
                    bdi.Descriptor = sd1;
                }
                if((long)bdi.Descriptor.Value == 4366631152)
                {
                    bdi.Descriptor = sd2;
                }
                //Main.logger.Log("Buff descriptor immunity list: " + bdi.Descriptor.Value.ToString());
            }

            //undead also have a derivative stat bonus immunity which they should retain
            //undead have immunitities to spells in a manner similar to buffs. They are immune to Any spell with a descriptor that matches their condition immunities 
            //The value is different here because the base game immunities do not include the VildravenBleed from the conditions immunities list
            //This block function similarly to the one above just for spell immunity instead of just condition immunity
            foreach (SpellImmunityToSpellDescriptor sitsd in undeadImmunities.GetComponents<SpellImmunityToSpellDescriptor>())
            {
                if((long)sitsd.Descriptor.Value == 2199640752384) //the initial value similar to above minus the value of the vildravn bleed. yes it is a 13 digit number thank you very much
                {
                    sitsd.Descriptor = sd1;
                }
                if ((long)sitsd.Descriptor.Value == 4366631152)
                {
                    sitsd.Descriptor = sd2;
                }
                //Main.logger.Log("Spell descriptor immunity list: " + sitsd.Descriptor.Value.ToString());
            }

            //because undead should be immune to all fortitude negates effects, they should have immunity to the baleful polymorph buff
            BlueprintBuff balefulPolymorphBuff = library.Get<BlueprintBuff>("0a52d8761bfd125429842103aed48b90");
            SpecificBuffImmunity sbi = new SpecificBuffImmunity
            {
                Buff = balefulPolymorphBuff
            };
            undeadImmunities.ComponentsArray = undeadImmunities.ComponentsArray.AddToArray(sbi);

            //BlueprintBuff dazed = library.Get<BlueprintBuff>("9934fedff1b14994ea90205d189c8759");//the buff from the daze spell
            //SpellDescriptorComponent sdc = dazed.GetComponent<SpellDescriptorComponent>();
            //sdc.Descriptor = SpellDescriptor.Daze;

            //foreach(BlueprintComponent bc in dazed.GetComponents<BlueprintComponent>())
            //{
            //    Main.logger.Log("Blueprint component for DazedBuff: " + bc.name + " and type: " + bc.GetType().ToString());
            //}
            //Main.logger.Log("DazedBuff spell descriptor component " + sdc.Descriptor.Value);


            //var undeadImmunitiesComponents = undeadImmunities.GetComponents<BlueprintComponent>();
            //foreach (BlueprintComponent bc in undeadImmunitiesComponents)
            //{
            //    Main.logger.Log("Undead immunities component name:" + bc.name + " and type:" + bc.GetType().ToString());
            //}

            //var deathWardBuff = library.Get<BlueprintBuff>("b0253e57a75b621428c1b89de5a937d1");
            //foreach(BlueprintComponent bc in deathWardBuff.GetComponents<BlueprintComponent>())
            //{
            //    Main.logger.Log("Deathward buff component name: " + bc.name + " and type: " +bc.GetType().ToString());
            //}

            //var undeadType = library.Get<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");
            //undeadType.GetComponent<SpellImmunityToSpellDescriptor>().Descriptor = SpellDescriptor.None;
            //undeadType.GetComponent<BuffDescriptorImmunity>().Descriptor = SpellDescriptor.None;
        }
    }
}



//[UndeadImmunitiesMod] Undead immunities component name:$AddConditionImmunity$e9468234-1521-4f8a-8d6d-610a35cf23c4 and type:Kingmaker.UnitLogic.FactLogic.AddConditionImmunity
//[UndeadImmunitiesMod] Undead immunities component name:$AddConditionImmunity$1941b6f7-6098-4360-a258-889cddd02c2b and type:Kingmaker.UnitLogic.FactLogic.AddConditionImmunity
//[UndeadImmunitiesMod] Undead immunities component name:$AddConditionImmunity$ffa417e1-0295-440a-82fd-278c11fa28c6 and type:Kingmaker.UnitLogic.FactLogic.AddConditionImmunity
//[UndeadImmunitiesMod] Undead immunities component name:$BuffDescriptorImmunity$eb929088-4f9e-4c60-92ee-89a0fa13d8f1 and type:Kingmaker.UnitLogic.FactLogic.BuffDescriptorImmunity
//[UndeadImmunitiesMod] Undead immunities component name:$BuffDescriptorImmunity$d4fb14f4-7d7b-45b3-ab7f-d7eb6f9f7a63 and type:Kingmaker.UnitLogic.FactLogic.BuffDescriptorImmunity
//[UndeadImmunitiesMod] Undead immunities component name:$DerivativeStatBonus$24891956-1323-478e-b0a4-b96472b79ad6 and type:Kingmaker.Designers.Mechanics.Facts.DerivativeStatBonus
//[UndeadImmunitiesMod] Undead immunities component name:$SpellImmunityToSpellDescriptor$c0976aae-8934-4994-9b1a-f5614f7d4f26 and type:Kingmaker.UnitLogic.FactLogic.SpellImmunityToSpellDescriptor
//[UndeadImmunitiesMod] Undead immunities component name:$SpellImmunityToSpellDescriptor$fb56d182-0078-4f5e-a1dd-5730215f7e72 and type:Kingmaker.UnitLogic.FactLogic.SpellImmunityToSpellDescriptor
//[UndeadImmunitiesMod] Undead immunities component name:$AddImmunityToAbilityScoreDamage$d886f4a4-2033-4ed0-bf23-14c29641d25b and type:Kingmaker.UnitLogic.FactLogic.AddImmunityToAbilityScoreDamage
//[UndeadImmunitiesMod] Undead immunities component name:$AddEnergyImmunity$75dd9c23-8027-4cd3-8753-dbe454f520e3 and type:Kingmaker.UnitLogic.FactLogic.AddEnergyImmunity

//old maybe useful code
//Common.undead.RemoveComponent()
//var undeadComponents = Common.undead.GetComponents<BlueprintComponent>();
//foreach (BlueprintComponent bc in undeadComponents)
//{
//    Main.logger.Log("Undead component name:" + bc.name + " and type:" + bc.GetType().ToString());
//}
//Main.logger.Log("The spell component descriptor for undead immunity" + Common.undead.GetComponent<BuffDescriptorImmunity>().name + " and the spell descriptor wrapper that it holds:" + Common.undead.GetComponent<BuffDescriptorImmunity>().Descriptor.ToString());
//Main.logger.Log("Its descriptor " + Common.undead.GetComponent<BuffDescriptorImmunity>().Descriptor.GetType().ToString() + " and its value: " + Common.undead.GetComponent<BuffDescriptorImmunity>().Descriptor.Value.ToString()); ;
//var undeadFacts = Common.undead.GetComponent<AddFacts>();
//foreach (BlueprintUnitFact fact in undeadFacts.Facts)
//{
//    Main.logger.Log("Undead facts name: " + fact.name + " description: " + fact.Description + " fancy name: " + fact.NameForAcronim);
//}
//BlueprintFeature undeadImmunities = (BlueprintFeature)Common.undead.GetComponent<AddFacts>().Facts[0];
//AddFacts undeadFacts = Common.undead.GetComponents<AddFacts>();
//BlueprintUnitFact[] Facts= new BlueprintUnitFact[]{ }; 
//foreach(BlueprintUnitFact)

//Base game Undead Immunities has 3 add condition immunities, 2 buff descriptor immunities, 1 derivative stat bonus
//2 spell immunitiy to spell descriptors, 1 add immunity to ability score damage, and 1 energy immunity
//var undeadComponents = library.Get<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33").GetComponents<BlueprintComponent>();
//            foreach (BlueprintComponent bc in undeadComponents)
//            {
//                Main.logger.Log("Undead component name:" + bc.name + " and type:" + bc.GetType().ToString());
//            }
//            var undeadImmunitiesComponents = undeadImmunities.GetComponents<BlueprintComponent>();
//            foreach (BlueprintComponent bc in undeadImmunitiesComponents)
//            {
//                Main.logger.Log("Undead immunities component name:" + bc.name + " and type:" + bc.GetType().ToString());
//            }