import { Player } from "./player.model";

export class Game {
    dealer: Player;
    human: Player;
    bots: Player[];
    deck: number[];
    options: string;
}
