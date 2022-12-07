using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unit04.Game.Casting;
using Unit04.Game.Directing;
using Unit04.Game.Services;



namespace Unit04
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        private static int FRAME_RATE = 12;
        private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private static int CELL_SIZE = 15;
        private static int COLS = 60;
        private static int ROWS = 40;
        private static int FONT_SIZE = 15;
        private static string CAPTION = "Robot Finds Kitten";
        private static string DATA_PATH = "Data/messages.txt";
        private static Color WHITE = new Color(255, 255, 255);
        private static int DEFAULT_ARTIFACTS = 40;
        private static int rand_x;
        
        
        public void GenerateObstacles(Cast cast)  
        {
            Random random = new Random();
            //set generic info for artifact
            string text = "F";
            int message = 4;
            //decide if the artifact will be a gem or a rock 
            int gemOrRock = random.Next(0,3);
            Point position = new Point(0, 0);
            position = position.Scale(15);
            rand_x = random.Next(900, 1800);
            if(gemOrRock==1){
                text = "1";
                Point _point1 = new Point(rand_x,500);
                position = _point1;
            }
            else if(gemOrRock == 2){
                text = "2";
                Point _point2 = new Point(rand_x,400);
                position = _point2;
            }
            else{
                text = "0";
                Point _point3 = new Point(rand_x,350);
                position = _point3;
                    
            }

            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);
            Color color = new Color(r, g, b);

            Artifact artifact = new Artifact();
            artifact.SetText(text);
            artifact.SetFontSize(15);
            artifact.SetColor(color);
            artifact.SetPosition(position);
            artifact.SetMessage(message);
            Point falling = new Point(-20, 0);
            artifact.SetVelocity(falling);
            cast.AddActor("artifacts", artifact);
        }


        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();

            // create the banner
            Actor banner = new Actor();
            Random random = new Random();
            
            
             
            
            banner.SetText(banner.getValue().ToString());

            banner.SetFontSize(FONT_SIZE);
            banner.SetColor(WHITE);
            banner.SetPosition(new Point(CELL_SIZE, 0));
            cast.AddActor("banner", banner);

            // create the robot
            Actor robot = new Actor();
            robot.SetText("#");
            robot.SetFontSize(FONT_SIZE);
            robot.SetColor(WHITE);
            robot.SetPosition(new Point(100, 500));
            cast.AddActor("robot", robot);

            // load the messages
            // List<int> messages = File.ReadAllLines(DATA_PATH).ToList<int>();


            // start the game
            KeyboardService keyboardService = new KeyboardService(CELL_SIZE);
            VideoService videoService 
                = new VideoService(CAPTION, MAX_X, MAX_Y, CELL_SIZE, FRAME_RATE, false);
            Director director = new Director(keyboardService, videoService);
            director.StartGame(cast);
        }
    }
}