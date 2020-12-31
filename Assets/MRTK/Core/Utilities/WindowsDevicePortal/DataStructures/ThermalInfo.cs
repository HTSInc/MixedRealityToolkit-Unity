// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Microsoft.MixedReality.Toolkit.WindowsDevicePortal
{
    [Serializable]
    public class ThermalInfo
    {
        /// <summary>
        ///  (0 normal, 1 warm, 2 critical)
        /// </summary>
        public int CurrentStage;
    }
}