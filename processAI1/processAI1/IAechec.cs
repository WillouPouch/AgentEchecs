using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace processAI1
{

    class IAechec
    {

        //-----------------------------------ATTRIBUTS
        //Objet Echiquier
        Echiquier echiquier;

        //Couleur de l'IA
        bool colorIA;

        //Meilleur move
        private String[] bestMove = new String[2];
        

        //-----------------------------------METHODES

        //---------------------------------------------------------------------------
        //      Constructeur
        //---------------------------------------------------------------------------
        public IAechec() {
            echiquier = new Echiquier();
        }


        //---------------------------------------------------------------------------
        //      Initialisation de l'échiquier
        //---------------------------------------------------------------------------
        public void InitEchiquier(int[] tabVal, String[] tabCoord) {

            //Blanc ou Noir
            echiquier.IsWhite(tabVal);

            //Créer échiquier 2D des valeurs pondérées des pièces (tabEval2D)
            echiquier.TabEvaluation(tabVal);

            //Créer échiquier 2D des coordonnées des pièces (tabCoord2D)
            echiquier.TabCoord2D(tabCoord);

            //Stocke la couleur de l'IA
            colorIA = echiquier.GetColorIA();

            //SI IA NOIR
            if (!echiquier.GetColorIA()) {
                echiquier.InversionEval2D(); //Inversion de l'échiquier
                echiquier.SetColorIA(true); //Et IA noir fait comme s'il était blanc
            }

        }


        //---------------------------------------------------------------------------
        //      Algo d'exploration : MiniMax
        //---------------------------------------------------------------------------
        public int MiniMax(int depth, int alpha, int beta) {
            //Double liste contenant tous les coups possibles
            List<List<string>> allMoves = new List<List<string>>();

            int[] posPiece = new int[2];
            int[] posMove = new int[2]; 
            int valPiece = 0; //obligation de l'initialiser...
            int valMove;
            bool originList = true; //permet de changer l'origine de la pièce quand on change de liste de coups
            string originMove = "";

            //Condition d'arrêt (profondeur limite fixée)
            if (depth == 0) {
               return echiquier.evaluateBoard();
            }

            //Récupérer tous les moves possibles
            allMoves = echiquier.GetAllMoves();

            //Max ou Min joueur
            if (echiquier.GetColorIA()) {
                int bestValue = -9999;

                foreach (var liste in allMoves) {
                    foreach (var move in liste) {
                        //1er element de la liste = origine des coups suivants
                        if (originList) {
                            originMove = move;
                            posPiece = echiquier.GetPosPiece(move);
                            valPiece = echiquier.GetTabEval(posPiece[0],posPiece[1]);
                            originList = false;
                        }
                        //Autres éléments de la liste
                        else {
                            posMove = echiquier.GetPosPiece(move);
                            valMove = echiquier.GetTabEval(posMove[0],posMove[1]);

                            //do move
                            echiquier.SetTabEval(posPiece[0], posPiece[1], 0);  //ancienne pos piece = 0
                            echiquier.SetTabEval(posMove[0], posMove[1], valPiece); //nouvelle pos piece = move

                            //MiniMax récursif
                            if (depth-1 != 0) echiquier.SetColorIA(!echiquier.GetColorIA()); //empecher le changement de tour lors de la prof max 
                            
                            //Profondeur initiale = garde le meilleur coup
                            if(depth == 3) {
                                int testMaxValue = MiniMax(depth - 1, alpha, beta);

                                alpha = Math.Max(alpha, testMaxValue);
                                
                                if (bestValue < testMaxValue) {
                                    bestValue = testMaxValue;
                                    bestMove[0] = originMove;
                                    bestMove[1] = move;
                                }
                            }
                            //Autres profondeurs = on s'intérèsse juste à la max value
                            else {
                                bestValue = Math.Max(bestValue, MiniMax(depth - 1, alpha, beta));

                                alpha = Math.Max(alpha, bestValue);

                                if (beta <= alpha) {
                                    //undo move
                                    echiquier.SetTabEval(posPiece[0], posPiece[1], valPiece); //on remet val piece à l'état de départ
                                    echiquier.SetTabEval(posMove[0], posMove[1], valMove); //on remet le move à l'état de départ
                                    echiquier.SetColorIA(!echiquier.GetColorIA());
                                    return bestValue;
                                }

                            }

                            //undo move
                            echiquier.SetTabEval(posPiece[0], posPiece[1], valPiece); //on remet val piece à l'état de départ
                            echiquier.SetTabEval(posMove[0], posMove[1], valMove); //on remet le move à l'état de départ
                        }
                    }
                    originList = true;
                }
                echiquier.SetColorIA(!echiquier.GetColorIA());
                return bestValue;
            }
            else {
                int bestValue = 9999;

                foreach (var liste in allMoves) {
                    foreach (var move in liste) {
                        //1er element de la liste = origine des coups suivants
                        if (originList) {
                            originMove = move;
                            posPiece = echiquier.GetPosPiece(move);
                            valPiece = echiquier.GetTabEval(posPiece[0], posPiece[1]);
                            originList = false;
                        }
                        //Autres éléments de la liste
                        else {
                            posMove = echiquier.GetPosPiece(move);
                            valMove = echiquier.GetTabEval(posMove[0], posMove[1]);

                            //do move
                            echiquier.SetTabEval(posPiece[0], posPiece[1], 0);  //ancienne pos piece = 0
                            echiquier.SetTabEval(posMove[0], posMove[1], valPiece); //nouvelle pos piece = moves

                            //MiniMax récursif
                            if (depth - 1 != 0) echiquier.SetColorIA(!echiquier.GetColorIA());
                            bestValue = Math.Min(bestValue, MiniMax(depth - 1, alpha, beta));

                            beta = Math.Min(beta, bestValue); 

                            if (beta <= alpha) {
                                //undo move
                                echiquier.SetTabEval(posPiece[0], posPiece[1], valPiece); //on remet val piece à l'état de départ
                                echiquier.SetTabEval(posMove[0], posMove[1], valMove); //on remet le move à l'état de départ
                                echiquier.SetColorIA(!echiquier.GetColorIA());

                                return bestValue;
                                }

                            //undo move
                            echiquier.SetTabEval(posPiece[0], posPiece[1], valPiece); //on remet val piece à l'état de départ
                            echiquier.SetTabEval(posMove[0], posMove[1], valMove); //on remet le move à l'état de départ
                        }
                    }
                    originList = true;
                }
                echiquier.SetColorIA(!echiquier.GetColorIA());
                
                return bestValue;
            }
        }


        //---------------------------------------------------------------------------
        //      Réinitilisation de la couleur par défaut de l'IA
        //---------------------------------------------------------------------------
        public void ResetColorIA() {
            echiquier.SetColorIA(colorIA);
        }


        //---------------------------------------------------------------------------
        //      Renverse le Best Move si IA noir
        //---------------------------------------------------------------------------
        public void ReverseBestMove(String[] tabCoord) {
            if (!echiquier.GetColorIA()) {
                InversePiece(tabCoord);
            }
        }

        
        //---------------------------------------------------------------------------
        //      Inversion du Best Move choisi (cas où IA est noir)
        //---------------------------------------------------------------------------
        public void InversePiece(String[] tabCoord) {
            int realOrigin = 0, realMove = 0;

            for (int i = 0; i < tabCoord.Length; i++) {
                if (tabCoord[i] == bestMove[0]) realOrigin = 63 - i;
                else if (tabCoord[i] == bestMove[1]) realMove = 63 - i;
            }

            bestMove[0] = tabCoord[realOrigin];
            bestMove[1] = tabCoord[realMove];
        }


        //---------------------------------------------------------------------------
        //      GET Best Move choisi
        //---------------------------------------------------------------------------
        public String[] GetBestMove() {
            return bestMove;
        }


        //---------------------------------------------------------------------------
        //      Vérification si BestMove implique une promotion
        //---------------------------------------------------------------------------
        public bool IsPromoted() {
            int[] originBM = echiquier.GetPosPiece(bestMove[0]); // BM = Best Move

            if ((originBM[0] == 1 && echiquier.GetColorIA() || originBM[0] == 6 && !echiquier.GetColorIA()) && echiquier.GetTabEval(originBM[0], originBM[1]) == 10) return true;
            else return false;
        }
    }
}
 