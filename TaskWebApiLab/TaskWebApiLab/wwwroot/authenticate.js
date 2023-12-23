// Assuming you have a way to decode JWT and extract username
// You might need a library or function to parse the JWT payload

// Run this script when the page loads
document.addEventListener('DOMContentLoaded', (event) => {
    const token = localStorage.getItem('jwtToken');

    if (!token) {
        // If the token isn't found, redirect back to the login page
        window.location.href = '/login.html';
    } else {
        // Decode JWT and set the username in the top-right corner
        // This is a placeholder for actual JWT decoding logic
        const username = getUsernameFromToken(token); // Implement this function based on your JWT structure
        document.getElementById('usernameDisplay').textContent = `Hello, ${username}`;
    }
});

function logout() {
    localStorage.removeItem('jwtToken');
    // Redirect to login page or refresh the page to trigger the authentication script
    window.location.href = '/login.html';
}

// Implement or import this function based on how your JWTs are structured
function getUsernameFromToken(token) {
    // Decode and return the username from the JWT
    // Placeholder for actual decoding logic
    return "Username"; // Replace with actual logic to decode JWT and extract username
}
