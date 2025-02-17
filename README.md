# Unity Drone Simulation with ML-Agents

## 📌 Project Overview
This project is a **drone simulation in Unity** that uses **Unity ML-Agents** for reinforcement learning. The drone follows a **multi-step process**:
1. **Takeoff** – The drone ascends to a target height.
2. **Navigation** – The drone moves toward a target end region.
3. **Landing** – The drone lands when it reaches the destination.

Rewards are given for reaching the correct height, moving toward the goal, and landing successfully. Penalties are applied for moving away from the goal or crashing.

---

## 📂 Project Structure
```
/Assets
  /Scripts
    - DroneAgent.cs        # ML-Agent controlling the drone
    - VelocityControl.cs    # Controls drone movement and physics
    - InputControl.cs       # Handles user input and movement
    - StateFinder.cs        # Retrieves drone state (altitude, velocity, etc.)
    - EnvironmentSetup.cs   # Defines the environment (optional)
  /ML-Agents
    - config.yaml          # ML training configuration file
  /Prefabs
    - Drone.prefab         # Prefab of the drone
    - StartRegion.prefab   # Starting position marker
    - EndRegion.prefab     # Target landing position
```

---

## 🔧 Setup & Installation
### **1️⃣ Install Unity & ML-Agents**
1. Download and install **Unity Hub** and **Unity 2021+**.
2. Open Unity and install the **ML-Agents package**:
   - Go to `Window > Package Manager`
   - Click `+ Add package from git URL`
   - Enter: `com.unity.ml-agents`

3. Install **Python & ML-Agents Toolkit**:
   ```sh
   pip install mlagents
   ```

---

### **2️⃣ Clone the Repository**
```sh
git clone https://github.com/YOUR-USERNAME/UnityDroneSim.git
cd UnityDroneSim
```

Open the project in **Unity**.

---

### **3️⃣ Train the Drone (ML-Agents)**
1. **Start Unity** and enter **Play Mode** to check if the drone moves correctly.
2. **Train the model using PPO (Proximal Policy Optimization)**:
   ```sh
   mlagents-learn config.yaml --run-id=drone_training --train
   ```
3. **Let the training run** until the agent successfully completes takeoff, navigation, and landing.
4. **Save the trained model** and place it inside Unity’s `Models/` folder.

---

### **4️⃣ Run the Trained Drone**
1. Attach the trained model `.onnx` to the `DroneAgent` GameObject.
2. In Unity, change the `Behavior Type` in `DroneAgent` to `Inference`.
3. Press **Play** in Unity to test the trained model.

---

## 🛠 Troubleshooting
| **Issue** | **Solution** |
|-----------|-------------|
| Drone is oscillating in height | Ensure `desiredHeight` updates smoothly in `DroneAgent.cs` |
| Drone does not move forward | Check if `velocityControl.desiredVX` is receiving action inputs |
| Training is slow | Increase `batch_size` in `config.yaml` and reduce environment complexity |
