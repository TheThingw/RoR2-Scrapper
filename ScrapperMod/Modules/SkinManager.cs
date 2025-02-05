using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scrapper.Modules
{
    internal static class SkinManager
    {
        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] defaultRendererInfos, GameObject root, UnlockableDef unlockableDef = null)
        {
            var newInfos = new CharacterModel.RendererInfo[defaultRendererInfos.Length];
            defaultRendererInfos.CopyTo(newInfos, 0);

            return CreateSkinDef(new SkinDefInfo
            {
                Icon = skinIcon,
                Name = skinName,
                NameToken = skinName,
                RendererInfos = newInfos,
                RootObject = root,
                UnlockableDef = unlockableDef,
                BaseSkins = [],
                GameObjectActivations = [],
                MeshReplacements = [],
                MinionSkinReplacements = [],
                ProjectileGhostReplacements = []
            });
        }

        internal static SkinDef CreateSkinDef(SkinDefInfo skinDefInfo)
        {
            On.RoR2.SkinDef.Awake += DoNothing;

            var skinDef = ScriptableObject.CreateInstance<SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }

        internal static CharacterModel.RendererInfo[] GetRendererMaterials(AssetBundle assetBundle, CharacterModel.RendererInfo[] defaultRenderers, params string[] materials)
        {
            var materialReplacements = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(materialReplacements, 0);

            for (var i = 0; i < defaultRenderers.Length; i++)
            {
                ref var info = ref defaultRenderers[i];
                if (!string.IsNullOrEmpty(materials.ElementAtOrDefault(i)))
                {
                    info.defaultMaterial = assetBundle.LoadAsset<Material>(materials[i]);
                }
            }

            return materialReplacements;
        }
        /// <summary>
        /// pass in strings for mesh assets in your bundle. pass the same amount and order based on your rendererinfos, filling with null as needed
        /// <code>
        /// myskindef.meshReplacements = Modules.Skins.getMeshReplacements(defaultRenderers,
        ///    "meshScrapperSword",
        ///    null,
        ///    "meshScrapper");
        /// </code>
        /// </summary>
        /// <param name="assetBundle">your skindef's rendererinfos to access the renderers</param>
        /// <param name="defaultRendererInfos">your skindef's rendererinfos to access the renderers</param>
        /// <param name="meshes">name of the mesh assets in your project</param>
        /// <returns></returns>
        internal static SkinDef.MeshReplacement[] GetMeshReplacements(AssetBundle assetBundle, CharacterModel.RendererInfo[] defaultRendererInfos, params string[] meshes)
        {
            var meshReplacements = new List<SkinDef.MeshReplacement>();

            for (var i = 0; i < defaultRendererInfos.Length; i++)
            {
                if (string.IsNullOrEmpty(meshes.ElementAtOrDefault(i)))
                    continue;

                meshReplacements.Add(new SkinDef.MeshReplacement
                {
                    renderer = defaultRendererInfos[i].renderer,
                    mesh = assetBundle.LoadAsset<Mesh>(meshes[i])
                });
            }

            return meshReplacements.ToArray();
        }
    }
}