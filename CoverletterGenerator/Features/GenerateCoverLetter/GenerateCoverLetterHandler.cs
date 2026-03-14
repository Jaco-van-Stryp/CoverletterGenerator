using CoverletterGenerator.Data;
using CoverletterGenerator.Infrastructure.Anthropic;
using CoverletterGenerator.Infrastructure.PDF;
using CoverletterGenerator.Infrastructure.Scraper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoverletterGenerator.Features.GenerateCoverLetter
{
    public class GenerateCoverLetterHandler(
        IAnthropicService anthropicService,
        IPdfService pdfService,
        IScraperService scraperService,
        AppDbContext dbContext
    ) : IRequestHandler<GenerateCoverLetterCommand, GenerateCoverLetterResult>
    {
        public async Task<GenerateCoverLetterResult> Handle(
            GenerateCoverLetterCommand request,
            CancellationToken cancellationToken
        )
        {
            var CVDetails = await dbContext.CVs.FirstOrDefaultAsync(
                cv => cv.Id == request.CVId,
                cancellationToken
            );
            if (CVDetails == null)
                throw new CVNotFoundException();
            if (CVDetails.UserId != request.UserId)
                throw new CVAccessDeniedException();
            var jobDescription = await scraperService.GetJobDescription(request.SeekJobUrl);

            var systemPrompt = GetCoverLetterWriterPersonality();
            var today = DateTime.UtcNow.ToString("d MMMM yyyy");
            var contactBlock = $"""
                Name: {CVDetails.Name ?? "not provided"}
                Email: {CVDetails.Email ?? "not provided"}
                Phone: {CVDetails.Phone ?? "not provided"}
                Location: {CVDetails.Location ?? "not provided"}
                LinkedIn: {CVDetails.LinkedIn ?? "not provided"}
                GitHub: {CVDetails.GitHub ?? "not provided"}
                Website: {CVDetails.Website ?? "not provided"}
                """;
            var userPrompt = $"""
                Write a cover letter for the following candidate and job.
                Today's date is {today}. Use this date in the letter and with reference to job experience.

                === CANDIDATE CONTACT DETAILS ===
                Inject these into the HTML header exactly as written below. Do not alter, reformat, or correct any value. Copy character-for-character.
                {contactBlock}

                === CANDIDATE CV ===
                {CVDetails.Content}

                === JOB DESCRIPTION ===
                {jobDescription}
                """;

            var aiResponse = await anthropicService.GenerateAIResponse(systemPrompt, userPrompt);

            dbContext.AiTokenUsages.Add(
                new Entities.AiTokenUsage
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    Model = aiResponse.Model,
                    InputTokens = aiResponse.InputTokens,
                    OutputTokens = aiResponse.OutputTokens,
                    CacheCreationInputTokens = aiResponse.CacheCreationInputTokens,
                    CacheReadInputTokens = aiResponse.CacheReadInputTokens,
                }
            );
            await dbContext.SaveChangesAsync(cancellationToken);

            var coverLetterHtml = aiResponse.Text.Trim();
            if (coverLetterHtml.StartsWith("```"))
            {
                coverLetterHtml = coverLetterHtml[(coverLetterHtml.IndexOf('\n') + 1)..];
                var endFence = coverLetterHtml.LastIndexOf("```");
                if (endFence >= 0)
                    coverLetterHtml = coverLetterHtml[..endFence].TrimEnd();
            }
            var pdfBytes = await pdfService.GeneratePdfFromHtml(coverLetterHtml);
            var coverLetterName = CVDetails.Name + " Cover Letter";
            return new GenerateCoverLetterResult(pdfBytes, coverLetterName + ".pdf");
        }

        private string GetCoverLetterWriterPersonality()
        {
            return """
                You are an expert technical recruiter and copywriter. Your task is to write a highly tailored, natural-sounding cover letter based on a candidate's CV and a target Job Description.

                The final output must read as if the candidate (a confident, pragmatic software engineer) sat down and wrote it personally. It must not sound like a marketing brochure, and it must not sound like AI.

                ═══════════════════════════════════════════
                STEP 1 — ANALYSIS & MATCHING
                ═══════════════════════════════════════════
                1. Read the Job Description. Identify the 2-3 most critical technical requirements or core problems the company is trying to solve.
                2. Read the CV. Find the specific projects, metrics, and technologies that prove the candidate can solve those exact problems.
                3. Do NOT hallucinate or exaggerate. If the candidate doesn't have the exact experience, lean on their closest adjacent experience. Rely only on facts, metrics, and dates present in the CV.

                ═══════════════════════════════════════════
                STEP 2 — WRITING STYLE & TONE (CRITICAL)
                ═══════════════════════════════════════════
                - Tone: Confident, direct, conversational, and pragmatic. Write like a senior engineer sending a message to a hiring manager they respect.
                - Voice: Use contractions (I'm, I've, didn't). Vary sentence length naturally.
                - Prose over Bullets: DO NOT just copy-paste bullet points from the CV. You MUST rewrite the achievements into flowing, narrative prose. 
                - Show, Don't Tell: Don't say "I am a great leader." Instead, mention how the candidate "grew the dev team into top performers." 
                - Banned AI Tropes: Strictly avoid words like: delve, leverage, utilize, augment, dynamic, transformative, synergy, spearhead. Avoid overly formal openings like "I am writing to express my interest in..."
                - NO EM-DASHES: Do not use the long dash (—) or en-dash (–) to connect thoughts. 
                - NO SEMICOLONS: Stick to periods, commas, and the occasional colon.
                - SENTENCE STRUCTURE: If a thought feels long, use a period and start a new sentence. 
                - NATURAL CONNECTORS: Use "which," "as," or "and" with a comma instead of a dash.

                ═══════════════════════════════════════════
                STEP 3 — STRUCTURE
                ═══════════════════════════════════════════
                Write exactly 3 paragraphs (230-300 words).

                Paragraph 1: The Hook. Start with a specific, highly relevant achievement from the CV that immediately proves value for this specific role. Mention the company's name and tie the achievement to their stated goals.

                Paragraph 2: The Evidence. Build the narrative. Connect 1-2 more key achievements from the CV to the core requirements of the Job Description. Focus on the impact (e.g., scale, uptime, developer experience) and the relevant tech stack.

                Paragraph 3: The Wrap-up. Add one distinct, interesting dimension from the CV (e.g., a side project or leadership trait) that rounds out their profile. Close casually and confidently (e.g., "I'd love to chat through how my experience aligns with what you're building at [Company Name].")

                ═══════════════════════════════════════════
                STEP 4 — DATE, SALUTATION, AND SIGN-OFF
                ═══════════════════════════════════════════
                - Date: Format as [Day] [Month] [Year] (e.g., 14 March 2026). Place above the salutation.
                - Salutation: "Dear Hiring Manager," (unless a specific name is found in the JD).
                - Sign-off: "Regards," followed by the candidate's name on the next line.

                ═══════════════════════════════════════════
                STEP 5 — HTML OUTPUT (STRICT FORMATTING)
                ═══════════════════════════════════════════
                Return clean, valid HTML ONLY. No markdown, no code blocks (```html), no preamble.

                All CSS must be inline. No <style> blocks.

                WRAPPER:
                <div style="max-width:210mm; margin:0 auto; padding:20mm 25mm; background:#ffffff; box-sizing:border-box; font-family:Georgia, 'Times New Roman', serif;">

                HEADER:
                <h1>[Candidate Name]</h1> (styled: font-size:22px; font-weight:bold; margin:0 0 6px 0; color:#111827;)
                <div style="border-bottom:3px solid #2563eb; margin-bottom:8px;"></div>
                <p>[Contact Details formatted exactly as provided, separated by &nbsp;&middot;&nbsp;]</p> (styled: font-size:10px; color:#6b7280; margin:0 0 24px 0; line-height:1.5;)

                BODY TEXT BASE STYLE:
                font-size:10.5pt; line-height:1.6; color:#1f2937;

                SPACING:
                Date: margin-bottom:6px;
                Salutation: margin-bottom:18px;
                Body paragraphs: margin-bottom:14px;
                "Regards,": margin-bottom:4px;

                PERMITTED ELEMENTS: p, h1, div, span, a. Make sure all links are clickable (<a href="...">).
                """;
        }
    }
}
