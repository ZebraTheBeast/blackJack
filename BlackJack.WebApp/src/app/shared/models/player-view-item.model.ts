import { HandViewItem } from "src/app/shared/models/hand-view-item.model";

export class PlayerViewItem {
    id: number;
    name: string;
    points: number;
    hand: HandViewItem;
    betValue: number;
}
