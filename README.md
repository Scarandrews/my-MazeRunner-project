#  👻 **Dandadan: Finding Golden Balls Maze** 👽 
## Sipnosis
Acompañanos a esta increíble aventura donde te embarcas en un laberinto para encontrar las bolas de oro de nuestro querido Okarun y te salvas de la maldición de la Turbo Ruca
## Condición de victoria del juego:
El juego consiste en recolectar cierta cantidad de objetivos (que serán las bolas perdidas de nuestro prota) y luego de cumplir con la cantidad establecida podrás salir por la puerta de la verdad (guiño guiño a Fullmetal Alchemist: Brotherhood), el primero que consiga esto será quien gane, en cambio si un jugador intenta salir por la puerta sin la cantidad establecida  perderá automaticamente (the balance must be respected).
## Instalación o formas de ejecución del juego: 
- Asegurate de tener instalado: Visual Studio o Visual Studio Code
- .NET SDK (versión 8.0 preferiblemente)
## Para descargarlo
Accede a la terminal de tu IDE o la misma terminal cmd y ejecuta el siguiente codigo: 
 ```
git clone  (instertar aqui el link del repositorio)
```
Ya esto seria suficiente para clonar el repositorio, solo quedaria ejecutar el codigo en su IDE de preferencia y :
```
dotnet run
```
## Lo que nos interesa:
## Breve descripcion del codigo
- ## Clases:
El codigo esta divido en varios archivos que representan las clases principales que conforman este proyecto:
- ### Program.cs :
  Contiene el metodo principal donde se hace el llamado a los metodos principales del programa y los ejecuta segun se flujo.
- ### MazeGenerator (esta divido en dos partial class por cuestion de orden y  limpieza del codigo):
  #### MazeGenerator.cs y MazeGeneratorMethods.cs:
  Contiene los metodos principales del programa
  - **PrintMaze** :
    Este metodo se encarga de imprimir el laberinto y mostrarlo en consola.
  - **GenerateMaze** :
    Genera el laberinto como el nombre indica, y lo inicializa.
  - **PlayGame**
    Inicia el laberinto gestiona los turnos de cada Jugador.
 - ### Player.cs
Representa a los jugadores en el laberinto y gestiona los turnos.
- ### Objectives.cs
Representa los objetivos en el laberinto 
- ### Trap.cs
  Representa las trampas en el laberinto
- ### Character.cs
  Representa los personajes de seleccion del juego
  ## En general:
  El algoritmo principal del juego es el que se utiliza para generar y gestionar el laberinto.
  A continuación, se describe el algoritmo paso a paso:
  - **Inicialización del laberinto**: El laberinto se inicializa con una matriz de celdas, donde cada celda puede ser una pared o un camino.
  - **Generación del laberinto**: El algoritmo utiliza un método de generación de laberintos llamado "algoritmo de recorrido aleatorio" (Randomized Depth-First Search). Este algoritmo funciona de la siguiente manera:
  Se selecciona una celda aleatoria en el laberinto y se marca como visitada.
  Se selecciona una celda adyacente a la celda actual que no haya sido visitada y se marca como visitada.
  Se repite el proceso hasta que se haya visitado todas las celdas del laberinto.
 - **Creación de caminos y paredes**: Una vez que se ha generado el laberinto, se crean caminos y paredes en función de las celdas visitadas. Las celdas que han sido visitadas se convierten en caminos, mientras que las celdas que no han sido visitadas se convierten en paredes.
  - **Colocación de obstáculos y trampas**: Se colocan obstáculos y trampas aleatorios en el laberinto. Los obstáculos pueden ser paredes o celdas que no pueden ser recorridas, mientras que las trampas pueden ser celdas que tienen un efecto negativo en el jugador.
  - **Colocación de objetivos**: Se colocan objetivos en el laberinto. Los objetivos pueden ser celdas que tienen un valor o un beneficio para el jugador.
    
    
    
  



