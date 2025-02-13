import { Board } from "./boards.types"

export type FindWordResults = {
    Board: Board
    XpGain: number
    WordsFound: string[]
}