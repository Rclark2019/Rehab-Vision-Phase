# Rehab Vision

A Unity-based rehabilitation analytics and visualization system that generates synthetic rehabilitation data and provides real-time 3D visualization without requiring actual VR hardware.

---

## 🎯 Project Overview
The system simulates rehabilitation sessions, tracks performance metrics, and provides comprehensive analytics through an interactive dashboard.

---

## ✨ Key Features

### 📊 Real-Time Analytics Dashboard
- **Performance Metrics**: Track accuracy, fatigue, and velocity in real-time
- **Live Graphing**: Visual representation of metrics over time with color-coded lines
- **Rep Counter**: Visual dot indicators showing completed repetitions
- **Session Information**: ID, timestamp, and difficulty ratings (1-5 stars)

### 🏃 Exercise Simulation
- **Multiple Exercises**: 
  - Jumping Jacks (15 reps, 4-star difficulty)
  - Arm Stretching (8 reps, 2-star difficulty)
- **Animated Avatar**: 3D character demonstrating proper exercise form
- **Automatic Rep Counting**: System tracks and counts repetitions automatically

### 🎮 Interactive Controls
- **Start/Stop**: Initialize and end rehabilitation sessions
- **Pause/Resume**: Interruption support with countdown timers
- **Exercise Switching**: Toggle between different exercise types
- **Session Reset**: Clear all data and restart

### 📈 Data Export & Analytics
- **CSV Export**: Save session data for further analysis
- **Performance Tracking**: Accuracy, fatigue, and velocity metrics
- **Historical Data**: Review past sessions and progress

###  User Interface
- **Clean Dashboard**: Professional metrics display with visual indicators
- **Status System**: Color-coded states (Ready, Active, Paused, Complete)
- **Help Popups**: Contextual information for each panel
- **Tutorial Tips**: Optional guidance with "don't show again" option
- **Avatar Feedback Bubbles**: Floating messages above avatar for guidance

---

##  Technical Stack

- **Engine**: Unity 6.2 (6000.0.9f1)
- **Language**: C# (.NET)
- **Animation**: Mixamo characters and animations
- **UI**: Unity UI Toolkit with TextMeshPro
- **Graphics**: Real-time texture-based performance graphs
- **Data Storage**: JSON serialization for session data

---

##  System Architecture

### Core Components

#### 1. ExerciseManager
Main controller handling:
- Session lifecycle (start, pause, resume, stop)
- Exercise switching and difficulty management
- Metric calculations and updates
- UI coordination

#### 2. RehabAvatarController
Animation system managing:
- Exercise animations (Idle, Jumping Jacks, Stretching)
- State transitions via Animator triggers
- Smooth animation blending

#### 3. PerformanceGraphTexture
Real-time graphing:
- Accuracy (green line)
- Fatigue (red line)
- Velocity (blue line)
- Dynamic texture rendering

#### 4. RepsDots
Visual rep counter:
- Dynamic dot generation based on exercise target
- Color-coded completion (grey → green)
- Automatic array resizing for different exercises

#### 5. SaveStats
Data persistence:
- JSON serialization
- Frame-by-frame metric recording
- CSV export functionality

---

##  Getting Started

### Prerequisites
- Unity 6.2 or higher
- Windows, Mac, or Linux OS
- Basic understanding of Unity Editor

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Rclark2019/Rehab-Vision-Phase.git
   cd rehab-vision
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Add" → Select project folder
   - Open with Unity 6.2+

3. **Open the main scene**
   - Navigate to `Assets/Scenes/`
   - Open `final.unity`

4. **Press Play**
   - Click "Start" to prepare session
   - Click "Switch Exercise" to begin first exercise
   - Use controls to manage the session

### Quick Start Guide

1. **Start Session**: Click "Start" button
   - Avatar enters Idle (waving) state
   - Metrics reset to initial values
   - System prepares for exercise

2. **Begin Exercise**: Click "Switch Exercise"
   - Countdown timer appears (3, 2, 1)
   - Avatar transitions to exercise animation
   - Metrics begin tracking

3. **Monitor Progress**:
   - Watch real-time metrics update
   - See dots fill in as reps complete
   - View performance graph

4. **Switch Exercise**: Click "Switch Exercise" again
   - Countdown appears
   - Avatar switches to different exercise
   - Reps reset for new exercise

5. **Complete Session**:
   - All reps complete automatically
   - Avatar returns to Idle state
   - "Session Complete" message appears

---

## 📊 Metrics Explained

### Accuracy (0-100%)
Measures how closely movements match target form
- **High (>80%)**: Excellent form
- **Medium (40-80%)**: Acceptable form
- **Low (<40%)**: Needs improvement, system provides feedback

### Fatigue (0-100%)
Energy depletion based on movement speed
- **Low (<30%)**: Fresh, energetic
- **Medium (30-70%)**: Moderate tiredness
- **High (>70%)**: Fatigued, system suggests break

### Velocity (0.8x-1.6x)
Current movement speed multiplier
- **Baseline**: 1.0x
- **Slower**: <1.0x
- **Faster**: >1.0x

---

## 🎮 Controls Reference

| Button | Function |
|--------|----------|
| **Start** | Initialize session, reset metrics |
| **Switch Exercise** | Begin/change exercise with countdown |
| **Pause** | Suspend current exercise |
| **Resume** | Continue paused exercise after countdown |
| **Save** | Export session data to JSON/CSV |
| **Reset** | Clear all data, return to initial state |

---
