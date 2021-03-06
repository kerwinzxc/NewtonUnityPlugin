﻿/*
* This software is provided 'as-is', without any express or implied
* warranty. In no event will the authors be held liable for any damages
* arising from the use of this software.
* 
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
* 
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software
* in a product, an acknowledgment in the product documentation would be
* appreciated but is not required.
* 
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
* 
* 3. This notice may not be removed or altered from any source distribution.
*/

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[DisallowMultipleComponent]
[AddComponentMenu("Newton Physics/Rigid Body")]
public class NewtonBody : MonoBehaviour
{
    //public bool Kinematic = false;
    //public bool KinematicCollidable = false;

    void Start()
    {
        var scripts = GetComponents<NewtonBodyScript>();
        foreach(var script in scripts)
        {
            m_scripts.Add(script);
        }
    }

    void OnDestroy()
    {
        if (m_world != null)
            m_world.UnregisterBody(this);

        // Destroy native body
        DestroyRigidBody();
    }

    // Update is called once per frame
    public void OnUpdateTranform()
    {
        IntPtr positionPtr = m_body.GetPosition();
        IntPtr rotationPtr = m_body.GetRotation();
        Marshal.Copy(positionPtr, m_positionPtr, 0, 3);
        Marshal.Copy(rotationPtr, m_rotationPtr, 0, 4);
        transform.position = new Vector3(m_positionPtr[0], m_positionPtr[1], m_positionPtr[2]);
        transform.rotation = new Quaternion(m_rotationPtr[1], m_rotationPtr[2], m_rotationPtr[3], m_rotationPtr[0]);
    }

    public void InitRigidBody()
    {
        m_collision = new NewtonBodyCollision(this);
        m_body = new dNewtonDynamicBody(m_world.GetWorld(), m_collision.GetShape(), Utils.ToMatrix(transform.position, transform.rotation), m_mass);

        var handle = GCHandle.Alloc(this);
        m_body.SetUserData(GCHandle.ToIntPtr(handle));

        m_world.RegisterBody(this);

    }

    public void DestroyRigidBody()
    {
        if (m_body != null)
        {
            var handle = GCHandle.FromIntPtr(m_body.GetUserData());
            handle.Free();

            m_body.Dispose();
            m_body = null;
        }

        if (m_collision != null)
        {
            m_collision.Destroy();
            m_collision = null;
        }
    }

    public dNewtonBody GetBody()
    {
        return m_body;
    }

    public void CalculateBuoyancyForces(Vector4 plane, ref Vector3 force, ref Vector3 torque, float bodyDensity)
    {
        if(m_body != null)
        {
            IntPtr planePtr = Marshal.AllocHGlobal(Marshal.SizeOf(plane));
            IntPtr forcePtr = Marshal.AllocHGlobal(Marshal.SizeOf(force));
            IntPtr torquePtr = Marshal.AllocHGlobal(Marshal.SizeOf(torque));

            Marshal.StructureToPtr(plane, planePtr, false);

            m_body.CalculateBuoyancyForces(planePtr, forcePtr, torquePtr, bodyDensity);

            force = (Vector3)Marshal.PtrToStructure(forcePtr, typeof(Vector3));
            torque = (Vector3)Marshal.PtrToStructure(torquePtr, typeof(Vector3));

            Marshal.FreeHGlobal(planePtr);
            Marshal.FreeHGlobal(forcePtr);
            Marshal.FreeHGlobal(torquePtr);
        }
    }

    public void ApplyExternaForces ()
    {
        // Apply force & torque accumulators
        m_body.AddForce(m_forceAcc.x, m_forceAcc.y, m_forceAcc.z);
        m_body.AddTorque(m_torqueAcc.x, m_torqueAcc.y, m_torqueAcc.z);
        m_forceAcc = Vector3.zero;
        m_torqueAcc = Vector3.zero;
    }

    public bool SleepState
    {
        get
        {
            if(m_body != null)
                return m_body.GetSleepState();

            return false;
        }
        set
        {
            if (m_body != null)
            {
                m_body.SetSleepState(value);
            }
        }
    }

    public float m_mass = 0.0f;
    public bool m_isScene = false;
    public NewtonWorld m_world;
    public Vector3 m_forceAcc { get; set; }
    public Vector3 m_torqueAcc { get; set; }

    internal dNewtonBody m_body = null;
    private NewtonBodyCollision m_collision = null;
    private float[] m_positionPtr = new float[3];
    private float[] m_rotationPtr = new float[4];

    internal List<NewtonBodyScript> m_scripts = new List<NewtonBodyScript>();

}
