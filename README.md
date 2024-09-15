<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Whack-a-Mole Game - README</title>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            background-color: #f0f8ff;
            color: #333;
            padding-bottom: 60px; /* Add space for footer */
        }
        header {
            background: linear-gradient(to right, #ff7e5f, #feb47b);
            color: #fff;
            padding: 20px;
            text-align: center;
            border-bottom: 5px solid #ff6f61;
        }
        header h1 {
            margin: 0;
            font-size: 2.5em;
        }
        img {
            max-width: 100%;
            height: auto;
            border: 5px solid #ff7e5f;
            border-radius: 10px;
            margin: 20px 0;
        }
        .container {
            width: 80%;
            margin: auto;
            overflow: hidden;
        }
        .main {
            padding: 20px;
            background: #ffffff;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin-bottom: 60px; /* Space for footer */
        }
        h2 {
            color: #ff6f61;
            border-bottom: 2px solid #ff7e5f;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }
        ul {
            list-style-type: square;
            padding-left: 20px;
        }
        ol {
            padding-left: 20px;
        }
        .footer {
            background: linear-gradient(to right, #ff7e5f, #feb47b);
            color: #fff;
            text-align: center;
            padding: 10px;
            position: fixed;
            bottom: 0;
            width: 100%;
            border-top: 5px solid #ff6f61;
        }
        a {
            color: #ff6f61;
            text-decoration: none;
        }
        a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <header>
        <div class="container">
            <h1>Whack-a-Mole</h1>
        </div>
    </header>

    <div class="container main">
        <img src="https://github.com/royg24/Whack-a-Mole/blob/main/Assets/Sprites/whackAMolePicture.jpg?raw=true" alt="Whack-a-Mole Game Screenshot">
        <h2>Project Overview</h2>
        <p>Whack-a-Mole is a fast-paced game where players must hit moles as they randomly pop up from various holes within a limited time. The game features a scoring system and a countdown timer to keep track of performance.</p>

        <h2>Developers</h2>
        <p>Roy Goldhar</p>
        <p>May Ashkenazi</p>

        <h2>Technologies Used</h2>
        <ul>
            <li>Unity</li>
            <li>C#</li>
        </ul>

        <h2>Getting Started</h2>
        <p>To get started with the Whack-a-Mole game:</p>
        <ol>
            <li>Clone the repository: <code>git clone https://github.com/royg24/Whack-a-Mole.git</code></li>
            <li>Open the project in Unity</li>
            <li>Build and run the game</li>
        </ol>
    </div>

    <footer class="footer">
        <p>&copy; 2024 Roy Goldhar and May Ashkenazi. All rights reserved.</p>
    </footer>
</body>
</html>
