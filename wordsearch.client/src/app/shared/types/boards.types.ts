import Difficulty from "./difficulty.type"
import { WordDictionary } from "./word-dictionary.types"

export type GameBoard = {
    Difficulty: Difficulty,
    BoardCharacters: string[][],
    Findable: WordDictionary,
    Found: string[]
    Started: number
}

export type Board = {
    Difficulty: Difficulty,
    BoardCharacters: string[][]
    Findable: string[]
    Found: WordDictionary
    Started: number
}
