# AGENTS.md

## Project goal
This repository contains a small Unity MVP for a kids event:
a rocket flies upward based on either microphone loudness or keyboard button hold.

## MVP scope
Build only what is needed for a working demo:
- one main scene
- rocket movement
- keyboard input mode
- microphone input mode
- processed normalized power value (0..1)
- simple UI for current power and result
- simple attempt flow and reset

## Do not build unless explicitly requested
- large frameworks
- dependency injection
- service locators
- event bus systems
- leaderboard persistence
- settings menus beyond what is necessary
- complex audio architecture
- custom editor tooling unless it saves clear time
- speculative abstractions for future features

## Coding style
- Keep code small, readable, and production-acceptable
- Prefer direct solutions over clever ones
- Use focused MonoBehaviours plus a few plain C# helper classes
- Avoid giant God objects, but also avoid splitting trivial logic into too many files
- Use SerializeField for Inspector-tuned values
- Keep naming explicit and boring
- Add comments only when intent is not obvious

## Architecture guidance
- Shared gameplay logic should work with both keyboard and microphone modes
- Avoid coupling raw microphone data directly to transform movement
- Keep one simple signal processing step between input and rocket movement
- Only introduce abstractions when they clearly reduce duplication or simplify the mic/keyboard swap

## Unity guidance
- Use deltaTime correctly for all time-based behavior
- Prefer Inspector-friendly parameters for tuning
- Keep scene hookup simple
- Summarize required object references after each task
- Prefer minimal runtime debug tools over heavy editor extensions

## Workflow for every task
1. Inspect the existing code first
2. Make a short implementation plan
3. Implement only what the task requires
4. Avoid unrelated refactors
5. Summarize:
   - changed files
   - scene hookup steps
   - manual testing notes

## Quality bar
This is a small freelance MVP:
- no hacks
- no dead code
- no obvious duplication if easy to remove
- no overengineering
- behavior should be stable and easy to tune
