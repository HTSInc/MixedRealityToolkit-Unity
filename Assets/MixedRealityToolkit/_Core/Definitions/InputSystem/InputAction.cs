﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Internal.Definitions.InputSystem
{
    /// <summary>
    /// An Input Action for mapping an action to an Input Sources Button, Joystick, Sensor, etc.
    /// </summary>
    [Serializable]
    public class InputAction : IMixedRealityInputAction
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Id for the InputAction</param>
        /// <param name="description">Human readable description for the action</param>
        public InputAction(uint id, string description)
        {
            this.id = id;
            this.description = description;
            this.axis = AxisType.None;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Id for the InputAction</param>
        /// <param name="description">Human readable description for the action</param>
        /// <param name="axisType">(Optional) limited Axis for the Action</param>
        public InputAction(uint id, string description, AxisType axisType)
        {
            this.id = id;
            this.description = description;
            this.axis = axisType;
        }

        /// <inheritdoc />
        public uint Id => id;

        [SerializeField]
        private uint id;

        /// <inheritdoc />
        public string Description => description;

        [SerializeField]
        private string description;

        /// <inheritdoc />
        public AxisType Axis => axis;

        [SerializeField]
        private readonly AxisType axis;
    }
}