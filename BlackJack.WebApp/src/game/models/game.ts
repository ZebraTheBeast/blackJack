import { Player } from "src/game/models/player";
import { Dealer } from "src/game/models/dealer";

export class Game {
    dealer: Dealer;
    human: Player;
    bots: Player[];
    deck: number[];
    options: string;
}
