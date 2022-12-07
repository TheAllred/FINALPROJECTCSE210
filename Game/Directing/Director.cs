using System;
using System.Collections.Generic;
using Unit04.Game.Casting;
using Unit04.Game.Services;


namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
         
         private static int CELL_SIZE = 15;
        private static int COLS = 60;
        private static int ROWS = 40;
         private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private KeyboardService _keyboardService = null;
        private VideoService _videoService = null;
        private Random random = new Random();
        private Point stopped = new Point(0, 0);
        private Point bottom = new Point(100, 500);

        private Point gravity = new Point(0,0);
        private Point gravityCONST = new Point(0,10);
        private int count = 0;
        private int rand_x;

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this._keyboardService = keyboardService;
            this._videoService = videoService;

        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            _videoService.OpenWindow();
            while (_videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            _videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void HandleCollisions(Cast cast){
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> artifacts = cast.GetActors("artifacts");

            


        }
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            
            if (robot.GetPosition().GetY()>480){
            gravity = gravity.Add(_keyboardService.GetDirection());}
            if (robot.GetPosition().GetY()<480){
            gravity = gravity.Add(gravityCONST);
            }
            if (robot.GetPosition().GetY()>485){
                gravity = stopped.Add(_keyboardService.GetDirection());
            }
            robot.SetVelocity(gravity);     
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> artifacts = cast.GetActors("artifacts");

            // banner.SetText(banner.getValue().ToString());
            banner.SetText(robot.GetPosition().GetY().ToString());
            // banner.SetText(gravity.GetY().ToString());
            int maxX = _videoService.GetWidth();
            int maxY = _videoService.GetHeight();
            robot.MoveNext(maxX, maxY);

            //ADD BASE LINE THAT ROBOT CANT GO BELOW 
             if (robot.GetPosition().GetY() > 500 ){
                    robot.SetPosition(bottom);
                }

                foreach (Actor actor in artifacts)
            {
                if (robot.GetPosition().Equals(actor.GetPosition()))
                {
                    Artifact artifact = (Artifact) actor;
                    int message = artifact.GetMessage();
                    // banner.addValue(message);
                    // banner.SetText(banner.getValue().ToString());
                    
                        int x = random.Next(1, COLS);
                    
                        int y = 0;
                        Point position = new Point(x, y);
                        // position = position.Scale(CELL_SIZE);
                    artifact.SetPosition(position);
                }
                actor.MoveNext(maxX, maxY);
               count += 1;
            } 
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            _videoService.ClearBuffer();
            _videoService.DrawActors(actors);
            if (count % 30 == 0)
            {
                GenerateObstacles(cast);
            }
            _videoService.FlushBuffer();
        }
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
                Point _point1 = new Point(MAX_X,500);
                position = _point1;
            }
            else if(gemOrRock == 2){
                text = "2";
                Point _point2 = new Point(MAX_X,400);
                position = _point2;
            }
            else{
                text = "0";
                Point _point3 = new Point(MAX_X,350);
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

    }
}