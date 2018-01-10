using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace processAI1
{
    class Echiquier
    {

        //-----------------------------------ATTRIBUTS

        //Si IA est blanc ou noir
        private bool isWhite;
        private bool premierTour = true;

        //Tableau 2D contenant les pieces de l'échiquier (val pondérées)
        private int[][] tabEval2D;

        //Tableau 2D contenant les coordonnées de l'échiquier (a8, c8, ...)
        private String[][] tabCoord2D;

        //Tableau des valeurs pondérées des pièces (pion, cavalier, fou, tour, reine, roi adverse, roi IA)
        private int[] valPiece = new int[] { 10, 30, 40, 50, 90, 900, 9000 };

        //---------------------------------------------------------------------------
        //Plus-value des pièces en fct de leur emplacement sur l'échiquier
        //---------------------------------------------------------------------------
        //PION
        private float[][] pion = new float[8][] {
            new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
            new float[8] { 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f },
            new float[8] { 1.0f, 1.0f, 2.0f, 3.0f, 3.0f, 2.0f, 1.0f, 1.0f },
            new float[8] { 0.5f, 0.5f, 1.0f, 2.5f, 2.5f, 1.0f, 0.5f, 0.5f },
            new float[8] { 0.0f, 0.0f, 0.0f, 2.0f, 2.0f, 0.0f, 0.0f, 0.0f },
            new float[8] { 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, -1.0f, -0.5f, 0.5f },
            new float[8] { 0.5f, 1.0f, 1.0f, -2.0f, -2.0f, 1.0f, 1.0f, 0.5f },
            new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }
        };
        //TOUR
        private float[][] tour = new float[8][] {
            new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f },
            new float[8] { 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.5f },
            new float[8] { -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -0.5f },
            new float[8] { -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -0.5f },
            new float[8] { -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -0.5f },
            new float[8] { -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -0.5f },
            new float[8] { -0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -0.5f },
            new float[8] { 0.0f, 0.0f, 0.0f, 0.5f, 0.5f, 0.0f, 0.0f, 0.0f }
        };
        //CAVALIER
        private float[][] cavalier = new float[8][] {
            new float[8] { -5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f },
            new float[8] { -4.0f, -2.0f, 0.0f, 0.0f, 0.0f, 0.0f, -2.0f, -4.0f },
            new float[8] { -3.0f, 0.0f, 1.0f, 1.5f, 1.5f, 1.0f, 0.0f, -3.0f },
            new float[8] { -3.0f, 0.5f, 1.5f, 2.0f, 2.0f, 1.5f, 0.5f, -3.0f },
            new float[8] { -3.0f, 0.0f, 1.5f, 2.0f, 2.0f, 1.5f, 0.0f, -3.0f },
            new float[8] { -3.0f, 0.5f, 1.0f, 1.5f, 1.5f, 1.0f, 0.5f, -3.0f },
            new float[8] { -4.0f, -2.0f, 0.0f, 0.5f, 0.5f, 0.0f, -2.0f, -4.0f },
            new float[8] { -5.0f, -4.0f, -3.0f, -3.0f, -3.0f, -3.0f, -4.0f, -5.0f }
        };
        //FOU
        private float[][] fou = new float[8][] {
            new float[8] { -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f },
            new float[8] { -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f },
            new float[8] { -1.0f, 0.0f, 0.5f, 1.0f, 1.0f, 0.5f, 0.0f, -1.0f },
            new float[8] { -1.0f, 0.5f, 0.5f, 1.0f, 1.0f, 0.5f, 0.5f, -1.0f },
            new float[8] { -1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, -1.0f },
            new float[8] { -1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, -1.0f },
            new float[8] { -1.0f, 0.5f, 0.0f, 0.0f, 0.0f, 0.0f, 0.5f, -1.0f },
            new float[8] { -2.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -1.0f, -2.0f }
        };
        //REINE
        private float[][] reine = new float[8][] {
            new float[8] { -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f },
            new float[8] { -1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f },
            new float[8] { -1.0f, 0.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.0f, -1.0f },
            new float[8] { -0.5f, 0.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.0f, -0.5f },
            new float[8] { 0.0f, 0.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.0f, -0.5f },
            new float[8] { -1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.0f, -1.0f },
            new float[8] { -1.0f, 0.0f, 0.5f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f },
            new float[8] { -2.0f, -1.0f, -1.0f, -0.5f, -0.5f, -1.0f, -1.0f, -2.0f }
        };
        //ROI
        private float[][] roi = new float[8][] {
            new float[8] { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f },
            new float[8] { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f  },
            new float[8] { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f  },
            new float[8] { -3.0f, -4.0f, -4.0f, -5.0f, -5.0f, -4.0f, -4.0f, -3.0f  },
            new float[8] { -2.0f, -3.0f, -3.0f, -4.0f, -4.0f, -3.0f, -3.0f, -2.0f },
            new float[8] { -1.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -2.0f, -1.0f },
            new float[8] { 2.0f, 2.0f, 0.0f, 0.0f, 0.0f, 0.0f, 2.0f, 2.0f },
            new float[8] { 2.0f, 3.0f, 1.0f, 0.0f, 0.0f, 1.0f, 3.0f, 2.0f }
        };



        //-----------------------------------METHODES

        //---------------------------------------------------------------------------
        //      Couleur de l'IA
        //---------------------------------------------------------------------------
        public void IsWhite(int[] tabVal) {
            //On questionne la couleur de l'IA qu'une seule fois
            if (premierTour) {
                //On parcourt les 4 lignes du milieu pour savoir si quelqu'un a joué
                for (int i = 16; i < 48; i++) {

                    //S'il n'y a pas que des 0, alors quelqu'un a joué donc on est "Noir"
                    if (tabVal[i] != 0) {
                        isWhite = false;
                        premierTour = false;
                        return;
                    }
                }
                isWhite = true; //Sinon on est "Blanc"
                premierTour = false;
            }
        }


        //---------------------------------------------------------------------------
        //      GETTER de la couleur de l'IA
        //---------------------------------------------------------------------------
        public bool GetColorIA() {
            return isWhite;
        }


        //---------------------------------------------------------------------------
        //      SETTER de la couleur de l'IA
        //---------------------------------------------------------------------------
        public void SetColorIA(bool color) {
            isWhite = color;
        }


        //---------------------------------------------------------------------------
        //     Créer tableau 2dim avec les valeurs pondérées des pièces
        //---------------------------------------------------------------------------
        public void TabEvaluation(int[] tabVal) {
            tabEval2D = new int[8][];
            int caseEchiquier = 0;

            for (int i = 0; i < 8; i++) {
                tabEval2D[i] = new int[8];

                for (int j = 0; j < 8; j++) {
                    tabEval2D[i][j] = PieceEvaluation(tabVal[caseEchiquier]);
                    caseEchiquier++;
                }
            }
        }


        //---------------------------------------------------------------------------
        //     GETTER de la valeur d'une case de tabEval2D
        //---------------------------------------------------------------------------
        public int GetTabEval(int i, int j) {
            return tabEval2D[i][j];
        }


        //---------------------------------------------------------------------------
        //     SETTER de la valeur d'une case de tabEval2D
        //---------------------------------------------------------------------------
        public void SetTabEval(int i, int j, int val) {
            tabEval2D[i][j] = val;
        }


        //---------------------------------------------------------------------------
        //     Poids en fonction du rôle de la pièce
        //---------------------------------------------------------------------------
        public int PieceEvaluation(int val) {
            int valPonderee;

            if (val == 1) valPonderee = valPiece[0];
            else if (val == -1) valPonderee = -valPiece[0];

            else if (val == 31 || val == 32) valPonderee = valPiece[1];
            else if (val == -31 || val == -32) valPonderee = -valPiece[1];

            else if (val == 4) valPonderee = valPiece[2];
            else if (val == -4) valPonderee = -valPiece[2];

            else if (val == 21 || val == 22) valPonderee = valPiece[3];
            else if (val == -21 || val == -22) valPonderee = -valPiece[3];

            else if (val == 5) valPonderee = valPiece[4];
            else if (val == -5) valPonderee = -valPiece[4];

            //On différencie roi IA et roi adverse car roi IA plus important
            else if (val == 6) {
                if (isWhite) valPonderee = valPiece[6]; //roi IA blanc
                else valPonderee = valPiece[5]; //roi adverse blanc
            }
            else if (val == -6) {
                if (!isWhite) valPonderee = -valPiece[6]; //roi IA noir
                else valPonderee = -valPiece[5]; //roi adverse noir
            }

            else valPonderee = 0;

            return valPonderee;
        }


        //---------------------------------------------------------------------------
        //      Transformer tabCoord en tableau 2dim
        //---------------------------------------------------------------------------
        public void TabCoord2D(String[] tabCoord) {
            tabCoord2D = new String[8][];
            int caseEchiquier = 0;

            for (int i = 0; i < 8; i++) {
                tabCoord2D[i] = new String[8];
                for (int j = 0; j < 8; j++) {
                    tabCoord2D[i][j] = tabCoord[caseEchiquier];
                    caseEchiquier++;
                }
            }
        }


        //---------------------------------------------------------------------------
        //      Récuperer position i,j d'une pièce
        //---------------------------------------------------------------------------
        public int[] GetPosPiece(String pieceCoord) {
            int[] pos = new int[2];

            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (pieceCoord == tabCoord2D[i][j]) {
                        pos[0] = i;
                        pos[1] = j;

                        return pos;
                    }
                }
            }
            return null; //obligé de faire un return sinon erreur (tous les chemins de code ne retournent pas tous une valeur)
        }


        //---------------------------------------------------------------------------
        //      Récuperer coordonnées d'une pièce
        //---------------------------------------------------------------------------
        public String GetCoordPiece(int posPieceI, int posPieceJ) {
            String pieceCoord = "";

            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (posPieceI == i && posPieceJ == j) {
                        pieceCoord = tabCoord2D[i][j];

                        return pieceCoord;
                    }
                }
            }
            return null; //obligé de faire un return sinon erreur (tous les chemins de code ne retournent pas tous une valeur)
        }


        //---------------------------------------------------------------------------
        //      Inversion des valeurs de tabEval2D (cas où IA est noir)
        //---------------------------------------------------------------------------
        public void InversionEval2D() {
            int[][] copieTabEval2D = new int[8][]; //rôle : copie de tabEval2D

            for (int i = 0; i < 8; i++) {
                copieTabEval2D[i] = new int[8];
                for (int j = 0; j < 8; j++) {
                    copieTabEval2D[i][j] = tabEval2D[i][j];
                }
            }

            //Inversion de tabEval2D via une copie de tabEval2D
            for (int i = 0, j = 7; i < 8; i++, j--) {
                for (int k = 0, l = 7; k < 8; k++, l--) {
                    tabEval2D[i][k] = -copieTabEval2D[j][l];
                }
            }
        }

       
        //---------------------------------------------------------------------------
        //      GET tous les moves possibles de toutes les pieces
        //---------------------------------------------------------------------------
        public List<List<string>> GetAllMoves() {

            //Double liste contenant tous les coups possibles pour un tour donné
            List<List<string>> allMoves = new List<List<string>>();

            int pieceVal;
            String pieceCoord;

            //Parcourt toutes les pièces de l'échiquier
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (isWhite && tabEval2D[i][j] > 0 || !isWhite && tabEval2D[i][j] < 0) {
                        //Valeur de la pièce
                        pieceVal = tabEval2D[i][j];

                        //Coord de la pièce
                        pieceCoord = GetCoordPiece(i, j);

                        //Récupérer i,j de la pièce en fonction de sa coord
                        int[] posPiece = GetPosPiece(pieceCoord);

                        //Ajout d'une sous-liste à la liste globale (tous les coups d'une pièce)
                        allMoves.Add(new List<string>(getMovesPiece(pieceVal, posPiece, pieceCoord)));
                    }
                }
            }

            return allMoves;
        }


        //---------------------------------------------------------------------------
        //      GET tous les moves possibles d'une pièce
        //---------------------------------------------------------------------------
        public List<string> getMovesPiece(int piece, int[] posPiece, String pieceCoord) {

            List<string> possibleMoves = new List<string>();
            possibleMoves.Add(pieceCoord); //premier élément de la liste = origine de la piece

            //Si IA joue en blanc et qu'on cherche les coups de l'adversaire
            //ou si IA joue en noir et qu'on cherche ses coups
            if (!isWhite) piece *= -1;

            //---------------------
            //Piece = PION
            //---------------------
            if (piece == 10) {
                //Tous les coups d'un pion (en fonction de la couleur des pièces)
                int[,] movesPion;
                if (isWhite) {
                    movesPion = new int[,] {
                         {-1, -1}, {-1, 1}, {-1, 0}, {-2, 0}
                    };
                }
                else {
                    movesPion = new int[,] {
                        {1, -1}, {1, 1}, {1, 0}, {2, 0}
                    };
                }

                //Vérification et ajout des coups valables
                for (int i = 0; i < 4; i++) {

                    int x = posPiece[0] + movesPion[i, 0];
                    int y = posPiece[1] + movesPion[i, 1];

                    //Si le coup est valable (dans l'échiquier)
                    if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                        //Si la case n'est pas occupée par une piece de l'IA
                        if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {

                            if (i == 2 && tabEval2D[x][y] == 0 ||
                                i == 3 && tabEval2D[x][y] == 0 && (isWhite && tabEval2D[x + 1][y] == 0 || !isWhite && tabEval2D[x - 1][y] == 0) && (isWhite && posPiece[0] == 6 || !isWhite && posPiece[0] == 1) ||
                                (i == 0 || i == 1) && (isWhite && tabEval2D[x][y] < 0 || !isWhite && tabEval2D[x][y] > 0)) {

                                //Le coup est valable
                                //int[] posPieceValable = { x, y };
                                possibleMoves.Add(GetCoordPiece(x, y));
                            }
                        }
                    }
                }
            }

            //---------------------
            //Piece = TOUR
            //---------------------
            if (piece == 50) {
                //Tous les coups d'une tour
                int[,] movesTour = new int[,] {
                   {0,-1}, {1,0}, {0,1}, {-1,0}
                };
                bool coupValable;
                int x = 0, y = 0;

                //Vérification et ajout des coups valables
                for (int i = 0; i < 4; i++) {
                    coupValable = true;
                    x = posPiece[0] + movesTour[i, 0];
                    y = posPiece[1] + movesTour[i, 1];

                    while (coupValable) {
                        //Si le coup est valable (dans l'échiquier)
                        if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                            //Si la case n'est pas occupée par une piece de l'IA
                            if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {
                                //Le coup est valable
                                //int[] posPieceValable = { x, y };
                                possibleMoves.Add(GetCoordPiece(x, y));

                                if (tabEval2D[x][y] == 0) {
                                    x += movesTour[i, 0];
                                    y += movesTour[i, 1];
                                }
                                else {
                                    coupValable = false;
                                }
                            }
                            else {
                                coupValable = false;
                            }
                        }
                        else {
                            coupValable = false;
                        }
                    }
                }
            }

            //---------------------
            //Piece = CAVALIER
            //---------------------
            if (piece == 30) {

                //Tous les coups d'un cavalier
                int[,] movesCavalier = new int[,] {
                    {-2, -1}, {-1, -2}, {1, -2}, {2, -1}, {2, 1}, {1, 2}, {-1, 2}, {-2, 1}
                };

                //Vérification et ajout des coups valables
                for (int i = 0; i < 8; i++) {

                    int x = posPiece[0] + movesCavalier[i, 0];
                    int y = posPiece[1] + movesCavalier[i, 1];

                    //Si le coup est valable (dans l'échiquier)
                    if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                        //Si la case n'est pas occupée par une piece de l'IA
                        if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {

                            //Le coup est valable
                            //int[] posPieceValable = { x, y };
                            possibleMoves.Add(GetCoordPiece(x, y));
                        }
                    }
                }
            }

            //---------------------
            //Piece = FOU
            //---------------------
            if (piece == 40) {
                //Tous les coups d'un fou
                int[,] movesFou = new int[,] {
                    {-1,-1}, {1,-1}, {1,1}, {-1,1}
                };
                bool coupValable;
                int x = 0, y = 0;

                //Vérification et ajout des coups valables
                for (int i = 0; i < 4; i++) {
                    coupValable = true;
                    x = posPiece[0] + movesFou[i, 0];
                    y = posPiece[1] + movesFou[i, 1];

                    while (coupValable) {
                        //Si le coup est valable (dans l'échiquier)
                        if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                            //Si la case n'est pas occupée par une piece de l'IA
                            if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {
                                //Le coup est valable
                                //int[] posPieceValable = { x, y };
                                possibleMoves.Add(GetCoordPiece(x, y));

                                if (tabEval2D[x][y] == 0) {
                                    x += movesFou[i, 0];
                                    y += movesFou[i, 1];
                                }
                                else {
                                    coupValable = false;
                                }
                            }
                            else {
                                coupValable = false;
                            }
                        }
                        else {
                            coupValable = false;
                        }
                    }
                }
            }

            //---------------------
            //Piece = REINE
            //---------------------
            if (piece == 90) {
                //Tous les coups d'une reine
                int[,] movesReine = new int[,] {
                    {-1,-1}, {0,-1}, {1,-1}, {1,0}, {1,1}, {0,1}, {-1,1}, {-1,0}
                };
                bool coupValable;
                int x = 0, y = 0;

                //Vérification et ajout des coups valables
                for (int i = 0; i < 8; i++) {
                    coupValable = true;
                    x = posPiece[0] + movesReine[i, 0];
                    y = posPiece[1] + movesReine[i, 1];

                    while (coupValable) {
                        //Si le coup est valable (dans l'échiquier)
                        if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                            //Si la case n'est pas occupée par une piece de l'IA
                            if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {
                                //Le coup est valable
                                //int[] posPieceValable = { x, y };
                                possibleMoves.Add(GetCoordPiece(x, y));

                                if (tabEval2D[x][y] == 0) {
                                    x += movesReine[i, 0];
                                    y += movesReine[i, 1];
                                }
                                else {
                                    coupValable = false;
                                }
                            }
                            else {
                                coupValable = false;
                            }
                        }
                        else {
                            coupValable = false;
                        }
                    }
                }
            }

            //---------------------
            //Piece = ROI
            //---------------------
            if (piece == 900 || piece == 9000) {
                //Tous les coups d'un roi
                int[,] movesRoi = new int[,] {
                    {-1,-1}, {0,-1}, {1,-1}, {1,0}, {1,1}, {0,1}, {-1,1}, {-1,0}
                };
                int x = 0, y = 0;

                //Vérification et ajout des coups valables
                for (int i = 0; i < 8; i++) {
                    x = posPiece[0] + movesRoi[i, 0];
                    y = posPiece[1] + movesRoi[i, 1];

                    //Si le coup est valable (dans l'échiquier)
                    if ((x < 8 && x >= 0) && (y < 8 && y >= 0)) {
                        //Si la case n'est pas occupée par une piece de l'IA
                        if ((isWhite && tabEval2D[x][y] <= 0) || (!isWhite && tabEval2D[x][y] >= 0)) {
                            //Le coup est valable
                            //int[] posPieceValable = { x, y };
                            possibleMoves.Add(GetCoordPiece(x, y));
                        }
                    }
                }
            }

            //Retourne les coups possibles de la pièce en paramètre
            return possibleMoves;
        }


        //---------------------------------------------------------------------------
        //      Evaluation d'un état de l'échiquier
        //---------------------------------------------------------------------------
        public int evaluateBoard() {
            int eval = 0;

            //Evaluation du coup potentiel
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    eval += poidsPosPiece(tabEval2D[i][j], i, j);
                }
            }

            return eval;
        }


        //---------------------------------------------------------------------------
        //      Calcul de la valeur d'une pièce avec une plus-value en fct de sa position
        //---------------------------------------------------------------------------
        public int poidsPosPiece(int valPiece, int posX, int posY) {
            float poidsPiece;

            //Cas d'une pièce "pion"
            if (valPiece == 10 || valPiece == -10) poidsPiece = (float)valPiece + pion[posX][posY];

            //Cas d'une pièce "tour"
            else if (valPiece == 50 || valPiece == -50) poidsPiece = (float)valPiece + tour[posX][posY];

            //Cas d'une pièce "cavalier"
            else if (valPiece == 30 || valPiece == -30) poidsPiece = (float)valPiece + cavalier[posX][posY];

            //Cas d'une pièce "fou"
            else if (valPiece == 40 || valPiece == -40) poidsPiece = (float)valPiece + fou[posX][posY];

            //Cas d'une pièce "reine"
            else if (valPiece == 90 || valPiece == -90) poidsPiece = (float)valPiece + reine[posX][posY];

            //Cas d'une pièce "roi"
            else if (valPiece == 900 || valPiece == -900) poidsPiece = (float)valPiece + roi[posX][posY];

            //Cas d'une case vide
            else poidsPiece = valPiece;

            return (int)poidsPiece;

            //return valPiece;
        }
    }
}