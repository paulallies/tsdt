using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

class Speaker
{

    // name of the speaker
    private String name;

    // number of occurrances
    private int occurrance = 1;

    public Speaker(String name)
    {
        this.name = name;
    }

    public String Name
    {
        get { return name; }
    }

    public int Occurrance
    {
        get { return occurrance; }
        set { occurrance = value; }
    }
}


class MyPageEvents : PdfPageEventHelper
{

    // we will keep a list of speakers
    SortedList speakers = new SortedList();

    // This is the contentbyte object of the writer
    PdfContentByte cb;

    // we will put the final number of pages in a template
    PdfTemplate template;

    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;

    // this is the current act of the play
    String act = "";

    // we override the onGenericTag method

    public override void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
    {
        if (speakers.Contains(text))
        {
            ((Speaker)speakers[text]).Occurrance++;
        }
        else
        {
            speakers.Add(text, new Speaker(text));
        }
    }


    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }
        catch (DocumentException de)
        {
        }
        catch (IOException ioe)
        {
        }
    }

    // we override the onChapter method
    public override void OnChapter(PdfWriter writer, Document document, float paragraphPosition, Paragraph title)
    {
        System.Text.StringBuilder buf = new System.Text.StringBuilder();
        foreach (Chunk chunk in title.Chunks)
        {
            buf.Append(chunk.Content);
        }
        act = buf.ToString();
    }




    // we override the onEndPage method
    public override void OnEndPage(PdfWriter writer, Document document)
    {

        int pageN = writer.PageNumber;
        String text = "Page " + pageN + " of ";
        float len = bf.GetWidthPoint(text, 8);
        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetTextMatrix(760, 25);
        cb.ShowText(text);
        cb.EndText();
        cb.AddTemplate(template, 760 + len, 25);
        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetTextMatrix(760, 820);
        if (pageN % 2 == 1)
        {
            cb.ShowText("Report");
        }
        else
        {
            cb.ShowText(act);
        }
        cb.EndText();
    }

    // we override the onCloseDocument method
    public override void OnCloseDocument(PdfWriter writer, Document document)
    {

        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.ShowText((writer.PageNumber - 1).ToString());
        template.EndText();
    }

    // we add a method to retrieve the glossary
    public SortedList Speakers
    {
        get
        {
            return speakers;
        }
    }
}


