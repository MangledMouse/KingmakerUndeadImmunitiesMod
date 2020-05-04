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

namespace UndeadImmunitiesMod
{

    class Rebalance
    {
        static LibraryScriptableObject library => Main.library;
        static public void undeadImmunitiesChange()
        {
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
            BlueprintFeature undeadImmunities = library.Get<BlueprintFeature>("8a75eb16bfff86949a4ddcb3dd2f83ae");
            //var undeadImmunitiesComponents = undeadImmunities.GetComponents<BlueprintComponent>();
            //foreach(BlueprintComponent bc in undeadImmunitiesComponents)
            //{
            //    Main.logger.Log("Undead immunities component name:" + bc.name + " and type:" + bc.GetType().ToString());
            //}
            foreach (AddConditionImmunity adc in undeadImmunities.GetComponents<AddConditionImmunity>())
            {
                Main.logger.Log("Add condition immunity: " + adc.Condition.ToString());
            }
            //undeadImmunities.RemoveComponents<AddConditionImmunity>();
            foreach (BuffDescriptorImmunity bdi in undeadImmunities.GetComponents<BuffDescriptorImmunity>())
            {
                Main.logger.Log("Buff descriptor immunity: " + bdi.Descriptor.Value.ToString());
            }
            //undeadImmunities.RemoveComponents<BuffDescriptorImmunity>();
            foreach (DerivativeStatBonus dsb in undeadImmunities.GetComponents<DerivativeStatBonus>())
            {
                Main.logger.Log("Derivative stat bonus: " + dsb.Descriptor.ToString());
            }
            //undeadImmunities.RemoveComponents<DerivativeStatBonus>();
            foreach (SpellImmunityToSpellDescriptor sitsd in undeadImmunities.GetComponents<SpellImmunityToSpellDescriptor>())
            {
                long theNumberIWant = (long)sitsd.Descriptor.Value;
                //string theValueIThinkIWant = ((Kingmaker.Blueprints.Classes.Spells.SpellDescriptor)(theNumberIWant + 0)).ToString();
                string theValueIThinkIWant = ((Kingmaker.Blueprints.Classes.Spells.SpellDescriptor)(theNumberIWant)).ToString();
                Main.logger.Log("Spell Immunity to Spell Descriptor: " + sitsd.Descriptor.ToString() + "its value is: " + theValueIThinkIWant);
            }
            //undeadImmunities.RemoveComponents<SpellImmunityToSpellDescriptor>();
            foreach (AddImmunityToAbilityScoreDamage aitasd in undeadImmunities.GetComponents<AddImmunityToAbilityScoreDamage>())
            {
                Main.logger.Log("Immunity to ability score damage includes drain: " + aitasd.Drain.ToString());

            }
            //undeadImmunities.RemoveComponents<AddImmunityToAbilityScoreDamage>();
            foreach (AddEnergyImmunity aei in undeadImmunities.GetComponents<AddEnergyImmunity>())
            {
                Main.logger.Log("" + aei.Type.ToString());
            }
            //undeadImmunities.RemoveComponents<AddEnergyImmunity>();
            //sick that worked!
        }
    }
}
