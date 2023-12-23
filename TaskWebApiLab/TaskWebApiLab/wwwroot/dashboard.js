// dashboard.js

// Base URL of your API
const apiBaseUrl = '/api/Goals';


async function fetchGoals() {
    try {
        const response = await fetch(`${apiBaseUrl}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
            }
        });

        if (response.ok) {
            const goals = await response.json();
            displayGoals(goals);  // Implement this function to update the UI
        } else {
            console.error('Failed to fetch goals:', response.status);
            // Handle errors
        }
    } catch (error) {
        console.error('Failed to fetch goals:', error);
        // Handle network errors
    }
}

function displayGoals(goals) {
    const goalsContainer = document.getElementById('goalsContainer');
    goalsContainer.innerHTML = ''; // Clear existing goals
    var g_data = goals.goals;
    // Create a map to hold the parent-children relationship
    const goalsMap = new Map(g_data.map(goal => [goal.id, { ...goal, children: [] }]));

    // Identify top-level goals and associate child goals
    let topLevelGoals = [];
    g_data.forEach(goal => {
        if (goal.parentTaskId === 0 || goal.parentTaskId === null) {
            topLevelGoals.push(goalsMap.get(goal.id));
        } else if (goalsMap.has(goal.parentTaskId)) {
            goalsMap.get(goal.parentTaskId).children.push(goalsMap.get(goal.id));
        }
    });

    // Now topLevelGoals contains all top-level goals, each with a 'children' property
    topLevelGoals.forEach(goal => {
        goalsContainer.appendChild(createGoalElement(goal));
    });
}

function createGoalElement(goal) {
    var dueTime = "NotSet";
    if (goal.dueTime !== null) {
        dueTime = goal.dueTime;
    }
    const element = document.createElement('div');
    element.className = 'goal';
    element.innerHTML = `
        <h3>Title: ${goal.title}</h3>
        <p>Description: ${goal.description}</p>
        <p>DueTime: ${dueTime}</p>
        <p>CratedAt: ${goal.createdAt}</p>
        <!-- Include other details as necessary -->
    `;

    if (goal.children && goal.children.length > 0) {
        const childrenContainer = document.createElement('div');
        childrenContainer.className = 'child-goals';
        goal.children.forEach(childGoal => {
            childrenContainer.appendChild(createGoalElement(childGoal)); // Recursive call for child goals
        });
        element.appendChild(childrenContainer);
    }

    return element;
}





// Function to fetch goals sorted by status
async function fetchGoalsByStatus(minimumStatus, maximumStatus) {
    try {
        // Construct the URL with query parameters for minimum and maximum status
        let url = `${apiBaseUrl}/goals-by-status`;
        const params = new URLSearchParams();
        if (minimumStatus !== undefined) {
            params.append('minimumStatus', minimumStatus);
        }
        if (maximumStatus !== undefined) {
            params.append('maximumStatus', maximumStatus);
        }
        url += `?${params.toString()}`;

        // Make the HTTP GET request to the server
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                'Content-Type': 'application/json'
            }
        });

        // Handle the response
        if (response.ok) {
            const goals = await response.json();
            // Do something with the goals, e.g., display them on the dashboard
            displayGoals(goals);  // Implement displayGoals to update the UI
        } else {
            console.error('Failed to fetch goals:', response.status);
            // Handle errors, e.g., by showing an error message to the user
        }
    } catch (error) {
        console.error('Failed to fetch goals by status:', error);
        // Handle network errors, e.g., by showing an error message to the user
    }
}

// Function to fetch goals sorted by category
async function fetchGoalsByCategory(categoryId) {
    // ... similar structure to fetchGoals, but include the query parameter for category
}

// Function to fetch a specific goal
async function fetchGoal(id) {
    // ... use GET /api/Goals/{id} to fetch a specific goal by ID
}

// Function to add a new goal
async function addGoal(newGoal) {
    // ... use POST /api/Goals with goal data in the body
}

// Function to update a goal
async function updateGoal(goalId, updatedGoal) {
    // ... use PUT /api/Goals/{id} with updated goal data
}

// Function to delete a goal
async function deleteGoal(goalId) {
    // ... use DELETE /api/Goals/{id}
}

async function submitNewGoal() {
    const url = `${apiBaseUrl}`; // Your API endpoint for creating a goal

    // Constructing the request body from form values
    const newGoal = {
        categoryId: document.getElementById('categoryId').value,
        parentTaskId: document.getElementById('parentTaskId').value || null,
        title: document.getElementById('title').value,
        description: document.getElementById('description').value,
        dueTime: document.getElementById('dueTime').value || null,
        status: parseInt(document.getElementById('status').value)
    };

    // Making the POST request to the server
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newGoal)
        });

        // Handling the response
        if (response.ok) {
            const addedGoal = await response.json();
            console.log('Goal added:', addedGoal);
            // Refresh goals or update the UI as needed
        } else {
            console.error('Failed to add goal:', response.status);
            // Handle errors
        }
    } catch (error) {
        console.error('Error adding goal:', error);
    }
}
// Function to log out the user
function logout() {
    localStorage.removeItem('jwtToken');
    window.location.href = '/login.html';
}

// Fetch all goals when the dashboard loads
document.addEventListener('DOMContentLoaded', async () => {
    // First, check if the user is logged in
    if (!localStorage.getItem('jwtToken')) {
        logout(); // or redirect to login
    } else {
        await fetchGoals();
    }
});

// Global variable to hold the current notification setting
let notificationHours = 1; // Default to 1 hour

// Function to update notification setting based on user selection
function updateNotificationSetting() {
    notificationHours = document.getElementById('notifyBefore').value;
    fetchNotifications(); // Fetch notifications immediately upon update
}

// Function to fetch notifications from the API
async function fetchNotifications() {
    const url = `/api/Notifications?hours=${notificationHours}`;

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
                // Add other necessary headers
            }
        });

        if (response.ok) {
            const notifications = await response.json();
            if (notifications.length > 0) {
                // Fetch detailed information for each notification
                const detailedGoalsPromises = notifications.map(notification =>
                    fetchGoalDetails(notification.goalId)
                );

                const detailedGoals = await Promise.all(detailedGoalsPromises);
                displayNotifications(notifications, detailedGoals); // Pass both notifications and detailed goals
            } else {
                displayNotifications([]); // No notifications
            }
        } else {
            console.error('Failed to fetch notifications:', response.status);
            // Handle errors
        }
    } catch (error) {
        console.error('Error fetching notifications:', error);
    }
}

async function fetchGoalDetails(goalId) {
    const url = `/api/Goals/${goalId}`;
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken')
                // Add other necessary headers
            }
        });

        if (response.ok) {
            return await response.json(); // Return the detailed goal
        } else {
            console.error('Failed to fetch goal details:', response.status);
            return null; // Return null if there's an error
        }
    } catch (error) {
        console.error('Error fetching goal details:', error);
        return null;
    }
}

// Function to display notifications
function displayNotifications(notifications, detailedGoals = []) {
    const notificationArea = document.getElementById('notificationArea');
    notificationArea.innerHTML = ''; // Clear previous notifications

    notifications.forEach((notification, index) => {
        const goalDetails = detailedGoals[index]; // Corresponding detailed goal info
        if (goalDetails) {
            // Create and append the notification element
            const div = document.createElement('div');
            div.className = 'notification';
            div.innerHTML = `
                <h4>Goal Alert: ${goalDetails.title}</h4>
                <p>Description: ${goalDetails.description}</p>
                <p>Due Time: ${notification.dueTime.toLocaleString()}</p>
            `;
            notificationArea.appendChild(div);
        }
    });

    if (notifications.length === 0) {
        notificationArea.innerHTML = '<p>No upcoming goals.</p>';
    }
}

// Fetch notifications every minute
setInterval(fetchNotifications, 60000); // 60000 milliseconds = 1 minute

// Initialize with default settings
document.addEventListener('DOMContentLoaded', () => {
    fetchNotifications(); // Fetch immediately when the page loads
});
