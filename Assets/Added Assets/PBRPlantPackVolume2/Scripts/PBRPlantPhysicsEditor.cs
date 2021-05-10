using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PBRPlantPhysicsEditor : EditorWindow
{
    //WINDOW VARS
    private bool configurableJointPropertyGroup;
    private bool rigidbodyPropertyGroup;
    private Vector2 scrollPosition;

    //CREATION OBJECT VARS
    private GameObject PlantBase;
    private List<GameObject> PlantBaseChildren = new List<GameObject>();

    //RIGIDBODY PROPERTIES
    private float RigidbodyMass = 0.01F;
    private bool RigidbodyUseGravity;
    private float RigidbodyAngDrag = 0.05F;
    private CollisionDetectionMode RigidbodyCollisionType = CollisionDetectionMode.Discrete;
    private float RigidbodyDrag = 0.25F;
    private RigidbodyInterpolation RigidbodyInterpolationType = RigidbodyInterpolation.None;
    private bool RigidbodyIsKinematic;

    //CONFIGURABLE JOINT PROPERTIES
    private const ConfigurableJointMotion AngularXMotion = ConfigurableJointMotion.Limited;
    private const ConfigurableJointMotion AngularYMotion = ConfigurableJointMotion.Limited;
    private const ConfigurableJointMotion AngularZMotion = ConfigurableJointMotion.Limited;
    private const RotationDriveMode rotationDriveMode = RotationDriveMode.Slerp;

    private float AngXLimitDamper = 5F;
    private float AngXLimitSping = 10F;
    private float AngYLimitBounce = 0.1F;
    private float AngYLimitContactDist;
    private float AngYLimitLim = 40F;
    private float AngYZLimitDamper = 5F;
    private float AngYZLimitSping = 10F;
    private float AngZLimitBounce = 0.1F;
    private float AngZLimitContactDist;
    private float AngZLimitLim = 40F;
    private float BreakForce = Mathf.Infinity;
    private float BreakTorque = Mathf.Infinity;
    private bool ConfiguredInWorldSpace;
    private bool EnableCollision;
    private bool EnablePreProcessing;
    private float HighAngXLimitBounce = 0.1F;
    private float HighAngXLimitContactDist;
    private float HighAngXLimitLim = 40F;
    private float LowAngXLimitBounce = 0.1F;
    private float LowAngXLimitContactDist;
    private float LowAngXLimitLim = -40F;
    private float ProjectionAngle = 180F;
    private float ProjectionDistance = 0.1F;
    private float SlerpDriveMaxForce = 10F;
    private float SlerpDrivePosDamper = 0.5F;
    private float SlerpDrivePosSpring = 10F;
    private bool SwapBodies;

    [MenuItem("Window/PBRPhysicsPlantEditor")]
    public static void ShowWindow()
    {
        GetWindow(typeof (PBRPlantPhysicsEditor));
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label("Tools to Quickly Update Rigidbody and Configurable joint properties.");
        PlantBase = (GameObject) EditorGUILayout.ObjectField("Base GameObject:", PlantBase, typeof (GameObject), true);
        if (GUILayout.Button("Update Configurable Joints and Rigidbodies"))
        {
            UpdateConfigurableJointsAndRigidbodies();
        }
        if (GUILayout.Button("Update Configurable Joints"))
        {
            UpdateConfigurableJoints();
        }
        if (GUILayout.Button("Update Rigidbodies"))
        {
            UpdateRigidbodies();
        }
        GUILayout.Label("-------------", EditorStyles.boldLabel);
        rigidbodyPropertyGroup = EditorGUILayout.BeginToggleGroup("Rigidbody Properties", rigidbodyPropertyGroup);
        if (rigidbodyPropertyGroup)
        {
            GUILayout.Label("See Unity manual online for description of properties.");
            RigidbodyMass = EditorGUILayout.FloatField("Mass:", RigidbodyMass);
            RigidbodyDrag = EditorGUILayout.FloatField("Drag:", RigidbodyDrag);
            RigidbodyAngDrag = EditorGUILayout.FloatField("Angular Drag:", RigidbodyAngDrag);
            RigidbodyUseGravity = EditorGUILayout.Toggle("Use Gravity:", RigidbodyUseGravity);
            RigidbodyIsKinematic = EditorGUILayout.Toggle("Is Kinematic:", RigidbodyIsKinematic);
            RigidbodyInterpolationType =
                (RigidbodyInterpolation)
                    EditorGUILayout.EnumPopup("Interpolation Type:", RigidbodyInterpolationType);
            RigidbodyCollisionType =
                (CollisionDetectionMode)
                    EditorGUILayout.EnumPopup("Collision Detection Type:", RigidbodyCollisionType);
        }
        EditorGUILayout.EndToggleGroup();
        configurableJointPropertyGroup = EditorGUILayout.BeginToggleGroup("Configurable Joint Properties",
            configurableJointPropertyGroup);
        if (configurableJointPropertyGroup)
        {
            GUILayout.Label("See Unity manual online for description of properties.");
            GUILayout.Label("Angular X Limit Spring:");
            AngXLimitSping = EditorGUILayout.FloatField("    Spring:", AngXLimitSping);
            AngXLimitDamper = EditorGUILayout.FloatField("    Damper:", AngXLimitDamper);
            GUILayout.Label("Low Angular X Limit:");
            LowAngXLimitLim = EditorGUILayout.FloatField("    Limit:", LowAngXLimitLim);
            LowAngXLimitBounce = EditorGUILayout.FloatField("    Bounciness:", LowAngXLimitBounce);
            LowAngXLimitContactDist = EditorGUILayout.FloatField("    Contact Distance:", LowAngXLimitContactDist);
            GUILayout.Label("High Angular X Limit:");
            HighAngXLimitLim = EditorGUILayout.FloatField("    Limit:", HighAngXLimitLim);
            HighAngXLimitBounce = EditorGUILayout.FloatField("    Bounciness:", HighAngXLimitBounce);
            HighAngXLimitContactDist = EditorGUILayout.FloatField("    Contact Distance:", HighAngXLimitContactDist);
        GUILayout.Label("Angular YZ Limit Spring:");
            AngYZLimitSping = EditorGUILayout.FloatField("    Spring:", AngYZLimitSping);
            AngYZLimitDamper = EditorGUILayout.FloatField("    Damper:", AngYZLimitDamper);
            GUILayout.Label("Angular Y Limit:");
            AngYLimitLim = EditorGUILayout.FloatField("    Limit:", AngYLimitLim);
            AngYLimitBounce = EditorGUILayout.FloatField("    Bounciness:", AngYLimitBounce);
            AngYLimitContactDist = EditorGUILayout.FloatField("    Contact Distance:", AngYLimitContactDist);
            GUILayout.Label("Angular Z Limit:");
            AngZLimitLim = EditorGUILayout.FloatField("    Limit:", AngZLimitLim);
            AngZLimitBounce = EditorGUILayout.FloatField("    Bounciness:", AngZLimitBounce);
            AngZLimitContactDist = EditorGUILayout.FloatField("    Contact Distance:", AngZLimitContactDist);
            if (rotationDriveMode == RotationDriveMode.Slerp)
            {
                GUILayout.Label("Slerp Drive:");
                SlerpDrivePosSpring = EditorGUILayout.FloatField("    Position Spring:", SlerpDrivePosSpring);
                SlerpDrivePosDamper = EditorGUILayout.FloatField("    Position Damper:", SlerpDrivePosDamper);
                SlerpDriveMaxForce = EditorGUILayout.FloatField("    Maximum Force:", SlerpDriveMaxForce);
            }
            ProjectionDistance = EditorGUILayout.FloatField("Projection Distance:", ProjectionDistance);
            ProjectionAngle = EditorGUILayout.FloatField("Projection Angle:", ProjectionAngle);
            ConfiguredInWorldSpace = EditorGUILayout.Toggle("Configured In World Space", ConfiguredInWorldSpace);
            SwapBodies = EditorGUILayout.Toggle("Swap Bodies:", SwapBodies);
            BreakForce = EditorGUILayout.FloatField("Break Force:", BreakForce);
            BreakTorque = EditorGUILayout.FloatField("Break Torque:", BreakTorque);
            EnableCollision = EditorGUILayout.Toggle("Enable Collision:", EnableCollision);
            EnablePreProcessing = EditorGUILayout.Toggle("EnablePreProcessiong:", EnablePreProcessing);
        }
        EditorGUILayout.EndToggleGroup();
        GUILayout.Label("Quick Start:");
        GUILayout.Label("1. Adjust properties in foldouts above");
        GUILayout.Label("2a. Set Base GameObject field above to single plant");
        GUILayout.Label("OR");
        GUILayout.Label("2b. Select multiple Plant GameObjects using Scene Hierarchy");
        GUILayout.Label("3. Click Update Buttons");
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void FindAllPlantPartsOfMesh()
    {
        if (PlantBase != null || Selection.objects != null)
        {
            if (PlantBaseChildren == null)
            {
                PlantBaseChildren = new List<GameObject>();
            }
            PlantBaseChildren.Clear();
            if (PlantBase != null)
            {
                PlantBaseChildren = GetAndAddChildrenToList(PlantBaseChildren, PlantBase.transform);
            }
            else if (Selection.objects != null)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    if (Selection.objects[i] is GameObject)
                    {
                        GameObject obj = Selection.objects[i] as GameObject;
                        GetAndAddChildrenToList(PlantBaseChildren, obj.transform);
                    }
                }
            }
        }
    }

    private List<GameObject> GetAndAddChildrenToList(List<GameObject> baseList, Transform parentTransform)
    {
        var childrenTotal = parentTransform.childCount;
        if (childrenTotal > 0)
        {
            for (var i = 0; i < childrenTotal; i++)
            {
                var childTransform = parentTransform.GetChild(i).gameObject;
                if (childTransform.transform.childCount > 0)
                {
                    Rigidbody rb = childTransform.GetComponent<Rigidbody>();
                    ConfigurableJoint configJoint = childTransform.GetComponent<ConfigurableJoint>();
                    if (rb != null && configJoint != null)
                    {
                        baseList.Add(childTransform);
                    }
                }
                GetAndAddChildrenToList(baseList, childTransform.transform);
            }
        }
        return baseList;
    }

    private void UpdateConfigurableJoints()
    {
        FindAllPlantPartsOfMesh();
        Undo.RegisterCompleteObjectUndo(PlantBaseChildren.ToArray(), "Modify Joints");
        ConfigureJoints();
    }

    private void ConfigureJoints()
    {
        foreach (var plantBone in PlantBaseChildren)
        {
            var childJoint = plantBone.GetComponent<ConfigurableJoint>();
            if (childJoint != null)
            {
                UpdateConfigurableJointParams(childJoint);
            }
        }
    }

    private void UpdateConfigurableJointParams(ConfigurableJoint joint)
    {
        joint.targetRotation = Quaternion.identity;
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;
        var tempSoftJointLimitSpring = new SoftJointLimitSpring();
        tempSoftJointLimitSpring.spring = AngXLimitSping;
        tempSoftJointLimitSpring.damper = AngXLimitDamper;
        joint.angularXLimitSpring = tempSoftJointLimitSpring;
        var tempSoftJointLimit = new SoftJointLimit();
        tempSoftJointLimit.bounciness = LowAngXLimitBounce;
        tempSoftJointLimit.limit = LowAngXLimitLim;
        tempSoftJointLimit.contactDistance = LowAngXLimitContactDist;
        joint.lowAngularXLimit = tempSoftJointLimit;
        tempSoftJointLimit.bounciness = HighAngXLimitBounce;
        tempSoftJointLimit.limit = HighAngXLimitLim;
        tempSoftJointLimit.contactDistance = HighAngXLimitContactDist;
        joint.highAngularXLimit = tempSoftJointLimit;
        tempSoftJointLimitSpring.spring = AngYZLimitSping;
        tempSoftJointLimitSpring.damper = AngYZLimitDamper;
        joint.angularYZLimitSpring = tempSoftJointLimitSpring;
        tempSoftJointLimit.bounciness = AngYLimitBounce;
        tempSoftJointLimit.limit = AngYLimitLim;
        tempSoftJointLimit.contactDistance = AngYLimitContactDist;
        joint.angularYLimit = tempSoftJointLimit;
        tempSoftJointLimit.bounciness = AngZLimitBounce;
        tempSoftJointLimit.limit = AngZLimitLim;
        tempSoftJointLimit.contactDistance = AngZLimitContactDist;
        joint.angularZLimit = tempSoftJointLimit;
        var tempJointDrive = new JointDrive();
        joint.rotationDriveMode = rotationDriveMode;
        tempJointDrive.positionSpring = SlerpDrivePosSpring;
        tempJointDrive.positionDamper = SlerpDrivePosDamper;
        tempJointDrive.maximumForce = SlerpDriveMaxForce;
        joint.slerpDrive = tempJointDrive;
        joint.projectionMode = JointProjectionMode.PositionAndRotation;
        joint.projectionDistance = ProjectionDistance;
        joint.projectionAngle = ProjectionAngle;
        joint.configuredInWorldSpace = ConfiguredInWorldSpace;
        joint.swapBodies = SwapBodies;
        joint.breakForce = BreakForce;
        joint.breakTorque = BreakTorque;
        joint.enableCollision = EnableCollision;
        joint.enablePreprocessing = EnablePreProcessing;
        joint.autoConfigureConnectedAnchor = true;
    }

    private void UpdateRigidbodies()
    {
        FindAllPlantPartsOfMesh();
        Undo.RegisterCompleteObjectUndo(PlantBaseChildren.ToArray(),"Modify Rigidbodies");
        ConfigureRigidbodies();
    }

    private void ConfigureRigidbodies()
    {
        foreach (var plantBone in PlantBaseChildren)
        {
            Rigidbody rigidBody = plantBone.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                UpdateRigidbodyValues(rigidBody);
            }
        }
    }

    private void UpdateRigidbodyValues(Rigidbody rigidBody)
    {
        var scaling = rigidBody.transform.lossyScale.x;
        rigidBody.mass = RigidbodyMass*scaling;
        rigidBody.drag = RigidbodyDrag*scaling;
        rigidBody.angularDrag = RigidbodyAngDrag*scaling;
        rigidBody.useGravity = RigidbodyUseGravity;
        rigidBody.isKinematic = RigidbodyIsKinematic;
        rigidBody.interpolation = RigidbodyInterpolationType;
        rigidBody.collisionDetectionMode = RigidbodyCollisionType;
    }

    void UpdateConfigurableJointsAndRigidbodies()
    {
        FindAllPlantPartsOfMesh();
        Undo.RegisterCompleteObjectUndo(PlantBaseChildren.ToArray(), "Modify Rigidbodies and Joints");
        ConfigureRigidbodies();
        ConfigureJoints();
    }
}