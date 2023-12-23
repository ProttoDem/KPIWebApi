// Check if the user is already logged in when the script loads
document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem('jwtToken');
    if (token) {
        // Hide the login form and show the logged in message
        document.getElementById('loginFormContainer').style.display = 'none';
        document.getElementById('loggedInMessage').style.display = 'block';
    }
});

document.getElementById('loginForm').onsubmit = async function (event) {
    event.preventDefault();

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('https://localhost:7127/api/Authenticate/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
        });

        const data = await response.json();

        if (data.token) {
            localStorage.setItem('jwtToken', data.token);
            window.location.href = '/dashboard.html';
        } else {
            document.getElementById('message').textContent = 'Login failed: ' + data.message;
        }
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('message').textContent = 'Login failed';
    }
};

function logout() {
    localStorage.removeItem('jwtToken');
    // Update UI or redirect as needed
    document.getElementById('loginFormContainer').style.display = 'block';
    document.getElementById('loggedInMessage').style.display = 'none';
}
