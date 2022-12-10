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
         private static Color WHITE = new Color(255, 255, 255);
        private static int FONT_SIZE = 15;
         private static int CELL_SIZE = 15;
         private static int MAX_X = 900;
        private KeyboardService _keyboardService = null;
        private VideoService _videoService = null;
        private RaylibAudioService _audioService = null;
        private Random random = new Random();
        private Point stopped = new Point(0, 0);
        private Point bottom = new Point(100, 500);

        private Point gravity = new Point(0,0);
        private Point gravityCONST = new Point(0,10);
        private int count = 0;
        private int rand_x;
        private bool gameIsRunning = true;
        private bool playAgain = false;
        private Point falling = new Point(-20, 0);
        


        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService, RaylibAudioService audioService)
        {
            this._keyboardService = keyboardService;
            this._videoService = videoService;
            this._audioService = audioService;

        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame()
        {
            Cast cast = new Cast();
            gameIsRunning = true;
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
            playAgain = false;
            _videoService.OpenWindow();
            _audioService.Initialize();
            _audioService.LoadSounds("Assets/Sound");
             while (_videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                HandleCollisions(cast);
                DoOutputs(cast);
                
            }
           
            
            // _audioService.UnloadSounds();

        }

        public void EndGame()
        {
            
            gameIsRunning = false;
          _videoService.DrawEndScreen();
        } 



        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void HandleCollisions(Cast cast){
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> artifacts = cast.GetActors("artifacts");
            Color RED = new Color(255, 0, 0);

              foreach (Actor actor in artifacts)
            {
                int actorX = actor.GetPosition().GetX()+(CELL_SIZE/2);
                int actorY = actor.GetPosition().GetY()+(CELL_SIZE/2);
                int robotX = robot.GetPosition().GetX()-5;
                int robotY = robot.GetPosition().GetY()-5;

            if (actorX<(robotX+CELL_SIZE)&&actorX>robotX&&actorY<(robotY+CELL_SIZE+5)&&actorY>robotY)
                {
                    robot.SetColor(RED);
                    EndGame();
                }
            } 
        }
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            playAgain = _keyboardService.EndScreenInput();
            if (gameIsRunning==false && playAgain==true){
            count = 0;
            robot.SetColor(WHITE);
            gameIsRunning = true;
            }

            if (robot.GetPosition().GetY()>480){
            gravity = gravity.Add(_keyboardService.GetDirection(_audioService));}

            if (robot.GetPosition().GetY()<480){
            gravity = gravity.Add(gravityCONST);
            }

            if (robot.GetPosition().GetY()>485){
                gravity = stopped.Add(_keyboardService.GetDirection(_audioService));
            }

            robot.SetVelocity(gravity);     
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            int maxX = _videoService.GetWidth();
            int maxY = _videoService.GetHeight();
            // Point speedUP = new Point(-(count/200), 0);
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> artifacts = cast.GetActors("artifacts");
            banner.SetText(count.ToString());
            if(gameIsRunning == true){
            robot.MoveNext(maxX, maxY);
            count += 1;
            }

            //ADD BASE LINE THAT ROBOT CANT GO BELOW 
             if (robot.GetPosition().GetY() > 500 ){
                    robot.SetPosition(bottom);
                }
                
            if(gameIsRunning == true){
                foreach (Actor actor in artifacts)
            {
                // Speed up artifacts-- Breaks collision handling
                // actor.SetVelocity(actor.GetVelocity().Add(speedUP));
                actor.MoveNext(maxX, maxY);
            } 
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
            int gemOrRock = random.Next(0,10);
            Point position = new Point(0, 0);
            position = position.Scale(15);
            rand_x = random.Next(900, 1800);
            if(gemOrRock <= 7){
                text = "@";
                Point _point1 = new Point(MAX_X,500);
                position = _point1;
            }
            else if(gemOrRock == 8){
                text = "@";
                Point _point2 = new Point(MAX_X,400);
                position = _point2;
            }
            else{
                text = "@";
                Point _point3 = new Point(MAX_X,350);
                position = _point3;
                    
            }

            int r = random.Next(50, 256);
            int g = random.Next(50, 256);
            int b = random.Next(50, 256);
            Color color = new Color(r, g, b);

            Artifact artifact = new Artifact();
            artifact.SetText(text);
            artifact.SetFontSize(15);
            artifact.SetColor(color);
            artifact.SetPosition(position);
            artifact.SetMessage(message);
            artifact.SetVelocity(falling);
            cast.AddActor("artifacts", artifact);
        }

    }
}