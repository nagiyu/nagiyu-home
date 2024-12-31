interface IPrivacyPolicy {
  title: string;
  contents: IPrivacyContent[];
}

interface IPrivacyContent {
  mainContent: string;
  subContents: IPrivacySubContent[];
  link: string;
}

interface IPrivacySubContent {
  subContent: string;
  subItems: string[];
}