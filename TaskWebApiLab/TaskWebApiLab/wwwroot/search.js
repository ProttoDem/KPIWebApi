async function searchGoals() {
    // Fetch values from the UI
    const minimumStatus = document.getElementById('minimumStatus').value;
    const maximumStatus = document.getElementById('maximumStatus').value;
    const categoryId = document.getElementById('category').value;

    // Construct the URL with query parameters
    let url = `api/Goals/goals-by-status-category`;
    const params = new URLSearchParams();
    if (minimumStatus) params.append('minimumStatus', minimumStatus);
    if (maximumStatus) params.append('maximumStatus', maximumStatus);
    if (categoryId) params.append('categoryId', categoryId);
    url += `?${params.toString()}`;

    // Fetch goals and update the UI
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const goals = await response.json();
            displaySearchResults(goals.goals);  // Function to handle UI update
        } else {
            console.error('Failed to fetch goals:', response.status);
            // Handle errors, maybe display a message to the user
        }
    } catch (error) {
        console.error('Error fetching goals:', error);
        // Handle network errors
    }
}

function displaySearchResults(goals) {
    const resultsContainer = document.getElementById('results');
    resultsContainer.innerHTML = ''; // Clear previous results

    // Assume each goal has an id and parentTaskId to determine the hierarchy
    let topLevelGoals = goals.filter(goal => !goal.parentTaskId || goal.parentTaskId === 0);
    let childGoals = goals.filter(goal => goal.parentTaskId && goal.parentTaskId !== 0);

    // Create a map of parent id to child goals for easy lookup
    let childGoalsMap = new Map();
    childGoals.forEach(child => {
        if (!childGoalsMap.has(child.parentTaskId)) {
            childGoalsMap.set(child.parentTaskId, []);
        }
        childGoalsMap.get(child.parentTaskId).push(child);
    });

    // Recursive function to create goal elements
    function createGoalElement(goal, isChild = false) {
        var dueTime = "NotSet";
        if (goal.dueTime !== null) {
            dueTime = goal.dueTime;
        }
        let goalElement = document.createElement('div');
        goalElement.className = isChild ? 'goal child' : 'goal';
        goalElement.innerHTML = `
            <h3>Title: ${goal.title}</h3>
            <p>Description: ${goal.description}</p>
            <p>DueTime: ${dueTime}</p>
            <p>CreatedAt: ${goal.createdAt}</p>
            <!-- Add other goal details here -->
        `;

        // Append child goals if any
        let childGoals = childGoalsMap.get(goal.id);
        if (childGoals && childGoals.length > 0) {
            let childContainer = document.createElement('div');
            childContainer.className = 'child-goals';
            childGoals.forEach(child => {
                childContainer.appendChild(createGoalElement(child, true));
            });
            goalElement.appendChild(childContainer);
        }

        return goalElement;
    }

    // Add each top-level goal and its children to the results container
    topLevelGoals.forEach(goal => resultsContainer.appendChild(createGoalElement(goal)));
}


// Function to fetch categories and populate the dropdown
async function fetchCategories() {
    try {
        const response = await fetch('/api/categories', {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                // Add additional headers as required by your API
            }
        });

        if (response.ok) {
            const categories = await response.json();
            populateCategoryDropdown(categories);
        } else {
            console.error('Failed to fetch categories:', response.status);
            // Handle errors, maybe display a message to the user
        }
    } catch (error) {
        console.error('Error fetching categories:', error);
        // Handle network errors
    }
}

// Function to populate the category dropdown
function populateCategoryDropdown(categories) {
    const categorySelect = document.getElementById('category');
    categories.forEach(category => {
        let option = document.createElement('option');
        option.value = category.id; // Assuming each category has an 'id'
        option.textContent = category.title; // And a 'name'
        categorySelect.appendChild(option);
    });
}

// Fetch categories when the document loads
document.addEventListener('DOMContentLoaded', () => {
    fetchCategories();
});


// Existing code in search.js
document.addEventListener('DOMContentLoaded', () => {
    fetchCategories();
    // Any other initialization code can go here
});
