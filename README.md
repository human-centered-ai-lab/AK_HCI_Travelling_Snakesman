# AK_HCI_Travelling_Snakesman

After you start the game you can choose between different sets of apples. Currently we have 3 testsets.
After you have selected one set you can hit the start button. Then the algorithm(self made MinMaxAntSystem algorithm) runs #apples * 5 iterations. 
After the computation you have the full control over a snake. You need to navigate the snake with the mouse on the PC or by touching the display on mobile devices. The goal of the game is to eat all the apples as fast as possible. So you need to try to find the shortest possible route over all apples. Every time you eat an apple the previous mentioned algorithm runs 5 iterations. The interactive part hereby is, that it doubles the pheromones on the travelled route.

So f.e. After you have eaten the third apple the algorithm has computed 15 iteration with a higher chance to choose edges you have travelled on eating those apples.

In the end you have following solutions.
The first algorithm computation (without human help) and the second algorithm computation (algorithm with human help).

The scientific goal of this game is solving the question: "Do we have a better performance by interacting with the algorithm?"