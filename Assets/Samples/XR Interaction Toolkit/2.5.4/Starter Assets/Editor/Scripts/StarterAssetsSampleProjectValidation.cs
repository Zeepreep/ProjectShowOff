using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;

namespace UnityEditor.XR.Interaction.Toolkit.Samples
{
    class StarterAssetsSampleProjectValidation : IPreprocessBuildWithReport
    {
        const string k_Category = "XR Interaction Toolkit";
        const string k_StarterAssetsSampleName = "Starter Assets";
        const string k_TeleportLayerName = "Teleport";
        const int k_TeleportLayerIndex = 31;

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!IsInteractionLayerTeleport())
            {
                DisplayTeleportDialog();
                // Optionally take corrective action here based on user input
                // For example:
                // InteractionLayerSettings.Instance.SetLayerNameAt(k_TeleportLayerIndex, k_TeleportLayerName);
            }
        }

        static bool IsInteractionLayerTeleport()
        {
            // Simulating the check without direct access to InteractionLayerSettings
            // Replace with actual logic if available
            string currentLayerName = LayerMask.LayerToName(k_TeleportLayerIndex);
            return currentLayerName.Equals(k_TeleportLayerName, StringComparison.OrdinalIgnoreCase);
        }

        static void DisplayTeleportDialog()
        {
            EditorUtility.DisplayDialog(
                "Fixing Teleport Interaction Layer",
                $"Interaction Layer {k_TeleportLayerIndex} for teleportation locomotion is currently set to '{LayerMask.LayerToName(k_TeleportLayerIndex)}' instead of '{k_TeleportLayerName}'",
                "OK");
        }
    }
}
