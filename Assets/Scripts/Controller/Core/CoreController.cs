    using UnityEngine;
    using Unity.Robotics.ROSTCPConnector;
    using Unity.Robotics.ROSTCPConnector.ROSGeometry;
    using RosMessageTypes.Geometry;

    public class CoreController : MonoBehaviour
    {
        private ShipController shipController;
        
        void Awake()
        {
            shipController = GetComponent<ShipController>();
            if (shipController == null)
            {
                Debug.LogError("ShipController component not found on " + gameObject.name);
            }
        }
        
        void Start()
        {
            try
            {
                ROSConnection.GetOrCreateInstance().Subscribe<TwistMsg>("/cmd_vel", ReceiveTwist);
                Debug.Log("Successfully subscribed to /cmd_vel topic");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to subscribe to ROS topic: " + e.Message);
            }
        }
        
        void ReceiveTwist(TwistMsg twistMsg)
        {
            if (twistMsg == null)
            {
                Debug.LogWarning("Received null TwistMsg");
                return;
            }
            
            float linearVelocity = (float)twistMsg.linear.x;
            float angularVelocity = (float)twistMsg.angular.z;
            
            if (shipController != null)
            {
                shipController.SetVel(linearVelocity, angularVelocity);
            }
            else
            {
                Debug.LogError("ShipController is null - cannot set velocities");
            }
        }
    }