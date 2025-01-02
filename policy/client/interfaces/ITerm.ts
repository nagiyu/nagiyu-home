interface ITerm {
  title: string;
  contents: ITermContent[];
}

interface ITermContent {
  mainContent: string;
  subItems: string[];
}