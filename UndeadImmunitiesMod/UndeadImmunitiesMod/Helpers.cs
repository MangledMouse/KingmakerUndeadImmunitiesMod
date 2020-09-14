using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndeadImmunitiesMod
{
    public static class Helpers
    {
        public static T CreateCopy<T>(this T original, Action<T> action = null) where T : UnityEngine.Object
        {
            var clone = UnityEngine.Object.Instantiate(original);
            if (action != null)
            {
                action(clone);
            }
            return clone;
        }

        public static void ReplaceComponent<T>(this BlueprintScriptableObject obj, Action<T> action) where T : BlueprintComponent
        {
            var replacement = obj.GetComponent<T>().CreateCopy();
            action(replacement);
            ReplaceComponent(obj, obj.GetComponent<T>(), replacement);
        }

        public static void ReplaceComponent(this BlueprintScriptableObject obj, BlueprintComponent original, BlueprintComponent replacement)
        {
            // Note: make a copy so we don't mutate the original component
            // (in case it's a clone of a game one).
            var components = obj.ComponentsArray;
            var newComponents = new BlueprintComponent[components.Length];
            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                if (c == original)
                    newComponents[i] = replacement;
                else
                    newComponents[i] = c;
                //newComponents[i] = c == original ? replacement : c;
            }
            obj.SetComponents(newComponents); // fix up names if needed
        }

        public static GameAction[] changeAction<T>(GameAction[] action_list, Action<T> change) where T : GameAction
        {
            //we assume that only possible actions are actual actions, conditionals, ContextActionSavingThrow or ContextActionConditionalSaved
            var actions = action_list.ToList();
            int num_actions = actions.Count();
            for (int i = 0; i < num_actions; i++)
            {
                if (actions[i] == null)
                {
                    continue;
                }
                else if (actions[i] is T)
                {
                    actions[i] = actions[i].CreateCopy();
                    change(actions[i] as T);
                    //continue;
                }

                if (actions[i] is Conditional)
                {
                    actions[i] = actions[i].CreateCopy();
                    (actions[i] as Conditional).IfTrue = Helpers.CreateActionList(changeAction<T>((actions[i] as Conditional).IfTrue.Actions, change));
                    (actions[i] as Conditional).IfFalse = Helpers.CreateActionList(changeAction<T>((actions[i] as Conditional).IfFalse.Actions, change));
                }
                else if (actions[i] is ContextActionConditionalSaved)
                {
                    actions[i] = actions[i].CreateCopy();
                    (actions[i] as ContextActionConditionalSaved).Succeed = Helpers.CreateActionList(changeAction<T>((actions[i] as ContextActionConditionalSaved).Succeed.Actions, change));
                    (actions[i] as ContextActionConditionalSaved).Failed = Helpers.CreateActionList(changeAction<T>((actions[i] as ContextActionConditionalSaved).Failed.Actions, change));
                }
                else if (actions[i] is ContextActionSavingThrow)
                {
                    actions[i] = actions[i].CreateCopy();
                    (actions[i] as ContextActionSavingThrow).Actions = Helpers.CreateActionList(changeAction<T>((actions[i] as ContextActionSavingThrow).Actions.Actions, change));
                }
            }

            return actions.ToArray();
        }

        public static ActionList CreateActionList(params GameAction[] actions)
        {
            if (actions == null || actions.Length == 1 && actions[0] == null) actions = Array.Empty<GameAction>();
            return new ActionList() { Actions = actions };
        }

        //public static ContextValue CreateContextValue(this AbilityRankType value)
        //{
        //    return new ContextValue() { ValueType = ContextValueType.Rank, ValueRank = value };
        //}
    }

    public static class ExtensionMethods
    {
        public static T Get<T>(this LibraryScriptableObject library, String assetId) where T : BlueprintScriptableObject
        {
            return (T)library.BlueprintsByAssetId[assetId];
        }

        public static void AddComponent(this BlueprintScriptableObject obj, BlueprintComponent component)
        {
            obj.SetComponents(obj.ComponentsArray.AddToArray(component));
        }

        public static void SetComponents(this BlueprintScriptableObject obj, params BlueprintComponent[] components)
        {
            // Fix names of components. Generally this doesn't matter, but if they have serialization state,
            // then their name needs to be unique.
            var names = new HashSet<string>();
            foreach (var c in components)
            {
                if (string.IsNullOrEmpty(c.name))
                {
                    c.name = $"${c.GetType().Name}";
                }
                if (!names.Add(c.name))
                {
                    //SaveCompatibility.CheckComponent(obj, c);
                    String name;
                    for (int i = 0; !names.Add(name = $"{c.name}${i}"); i++) ;
                    c.name = name;
                }
                //Log.Validate(c, obj);
            }

            obj.ComponentsArray = components;
        }
    }
}
